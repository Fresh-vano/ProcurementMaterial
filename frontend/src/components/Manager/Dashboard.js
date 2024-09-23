import React, { useState, useEffect } from 'react';
import { Container, Typography, Grid, Button, AppBar, Toolbar, Paper, FormControl, MenuItem, Select, InputLabel } from '@mui/material';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ArcElement,
  PointElement,
  LineElement,
  LineController,
  BarController,
  PieController,
} from 'chart.js';
import { Bar, Line } from 'react-chartjs-2';
import { useAuth } from '../../utils/auth';
import { useNavigate } from 'react-router-dom';
import api from '../../api';
import jsPDF from 'jspdf';
import html2canvas from 'html2canvas';
import Tile from '../../Tile';
import LogoutButton from '../Auth/LogoutButton';

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
  ArcElement,
  PointElement,
  LineElement,
  LineController,
  BarController,
  PieController
);

const ManagerDashboard = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const [selectedGroup, setSelectedGroup] = useState('');
  const [groupOptions, setGroupOptions] = useState([]);
  const [groupChartData, setGroupChartData] = useState({ labels: [], datasets: [] });
  const [chartsData, setChartsData] = useState({});

  useEffect(() => {
    const fetchGroupOptions = async () => {
      try {
        const response = await api.get('http://localhost:8080/chart/GetUniqueGroup');
        setGroupOptions(response.data);
      } catch (error) {
        console.error('Error fetching group options:', error);
      }
    };

    fetchGroupOptions();
  }, []);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const receiptsAndStocksResponse = await api.get('http://localhost:8080/chart/GenerateReceiptsAndStocksChartJson');
        const monthlyExpensesResponse = await api.get('http://localhost:8080/chart/GenerateMonthlyExpensesChartJson');

        setChartsData({
          receiptsAndStocks: receiptsAndStocksResponse.data.chart,
          monthlyExpenses: monthlyExpensesResponse.data.chart,
        });
      } catch (error) {
        console.error('Error fetching chart data:', error);
      }
    };

    fetchData();
  }, []);

  const handleGroupChange = async (event) => {
    const selected = event.target.value;
    setSelectedGroup(selected);

    try {
      const response = await api.post('http://localhost:8080/chart/GenerateReceiptsAndStocksChartJson', { MaterialName: selected });
      setGroupChartData(response.data.chart);
    } catch (error) {
      console.error('Error fetching group chart data:', error);
    }
  };

  const handleDownloadPDF = async () => {
    const input = document.getElementById('pdfContent');
    const canvas = await html2canvas(input);
    const imgData = canvas.toDataURL('image/png');
    const pdf = new jsPDF();
    const imgProps = pdf.getImageProperties(imgData);
    const pdfWidth = pdf.internal.pageSize.getWidth();
    const pdfHeight = (imgProps.height * pdfWidth) / imgProps.width;
    pdf.addImage(imgData, 'PNG', 0, 0, pdfWidth, pdfHeight);
    pdf.save('dashboard.pdf');
  };

  return (
    <>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" style={{ flexGrow: 1 }}>
            Страница руководителя
          </Typography>
          <LogoutButton/>
          <Button color="inherit" onClick={handleDownloadPDF}>
            Сохранить в PDF
          </Button>
        </Toolbar>
      </AppBar>
      <Container id="pdfContent">
        <Grid container spacing={3}>
          <Grid item xs={12}>
            <Tile>
              <Paper sx={{ p: 2 }}>
                {chartsData.receiptsAndStocks && 
                <>
                  <Typography variant="h6" style={{ flexGrow: 1 }}>
                    Сводная таблица поступлений и запасов
                  </Typography>
                  <Bar data={chartsData.receiptsAndStocks} />
                </>
                }
              </Paper>
            </Tile>
          </Grid>
          <Grid item xs={12}>
            <Tile>
              <Paper sx={{ p: 2 }}>
                {chartsData.monthlyExpenses && 
                <>
                  <Typography variant="h6" style={{ flexGrow: 1 }}>
                    Сводная таблица суммарных трат в месяц
                  </Typography>
                  <Bar data={chartsData.monthlyExpenses} />
                </>
                }
              </Paper>
            </Tile>
          </Grid>
          <Grid item xs={12}>
          <Paper sx={{ p: 2, width: '100%' }}>

            <FormControl fullWidth>
              <Typography id="group-select-label">Выберите группу</Typography>
              <Select
                labelId="group-select-label"
                id="group-select"
                value={selectedGroup}
                onChange={handleGroupChange}
                MenuProps={{
                  PaperProps: {
                    style: {
                      maxHeight: 200, // Задание maxHeight для всплывающего меню
                    },
                  },
                }}
              >
                {groupOptions.map((group) => (
                  <MenuItem key={group} value={group}>
                    {group}
                  </MenuItem>
                ))}
              </Select>
            </FormControl>
            <Paper sx={{ p: 2, mt: 2 }}>
              <Typography variant="h6" style={{ flexGrow: 1 }}>
                График по выбранной группе
              </Typography>
              <Bar data={groupChartData} />
            </Paper>
            </Paper>
          </Grid>
        </Grid>
      </Container>
    </>
  );
};

export default ManagerDashboard;
