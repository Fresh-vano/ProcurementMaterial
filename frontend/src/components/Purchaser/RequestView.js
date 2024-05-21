import React, { useState } from 'react';
import { AppBar, Toolbar, Typography, Button, Container, Paper, Grid, MenuItem, FormControl, InputLabel, Select, Box } from '@mui/material';
import {
  Chart as ChartJS,
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend,
} from 'chart.js';
import { useAuth } from '../../utils/auth';
import { useNavigate } from 'react-router-dom';
import { Bar } from 'react-chartjs-2';
import axios from 'axios';

ChartJS.register(
  CategoryScale,
  LinearScale,
  BarElement,
  Title,
  Tooltip,
  Legend
);

const PurchaserRequestView = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const [selectedMaterial, setSelectedMaterial] = useState('');
  const [selectedInns, setSelectedInns] = useState([]);
  const [chartData, setChartData] = useState(null);

  const materialOptions = [
    '1200036086',
    '1000000028',
    '1000000029',
    '1000000031',
    '1000000032',
  ];

  const innOptions = [
    '7711079534',
    '7711079535',
    '5407970463',
  ];

  const generateChartData = async () => {
    try {
      const response = await axios.post('http://localhost:8080/chart/4GenerateSupplierCostComparisonChartJson', {
        material: selectedMaterial,
        INNs: selectedInns
      });
      setChartData(response.data.bar);
    } catch (error) {
      console.error('Error fetching chart data:', error);
    }
  };

  const handleMaterialChange = (event) => {
    setSelectedMaterial(event.target.value);
    setSelectedInns([]); // Reset selected INNs when material changes
  };

  const handleInnChange = (event) => {
    setSelectedInns(event.target.value);
  };

  const handleLogout = () => {
    logout(() => navigate('/login'));
  };

  const data = {
    labels: selectedInns,
    datasets: [
      {
        label: 'Average Price',
        data: selectedInns.map((inn) => Math.floor(Math.random() * 100)), // Здесь нужно заменить на реальные данные
        backgroundColor: selectedInns.map(() => `rgba(${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, ${Math.floor(Math.random() * 255)}, 0.6)`),
      },
    ],
  };

  return (
    <>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" style={{ flexGrow: 1 }}>
            Страница закупщиков
          </Typography>
          <Button color="inherit" onClick={handleLogout}>
            Выход
          </Button>
        </Toolbar>
      </AppBar>
      <Container>
        <Box style={{ marginTop: '20px', padding: '20px' }}>
          <FormControl fullWidth>
            <InputLabel id="material-select-label">Материал</InputLabel>
            <Select
              labelId="material-select-label"
              id="material-select"
              value={selectedMaterial}
              onChange={handleMaterialChange}
            >
              {materialOptions.map((material) => (
                <MenuItem key={material} value={material}>
                  {material}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
        </Box>
        {selectedMaterial && (
          <>
            <Box style={{ marginTop: '20px', padding: '20px' }}>
              <FormControl fullWidth>
                <InputLabel id="inn-select-label">ИНН</InputLabel>
                <Select
                  labelId="inn-select-label"
                  id="inn-select"
                  multiple
                  value={selectedInns}
                  onChange={handleInnChange}
                  renderValue={(selected) => selected.join(', ')}
                >
                  {innOptions.map((option) => (
                    <MenuItem key={option} value={option}>
                      {option}
                    </MenuItem>
                  ))}
                </Select>
              </FormControl>
            </Box>
            <Button 
              variant="contained" 
              color="primary" 
              onClick={generateChartData}
              style={{ marginTop: '20px' }}
            >
              Создать график
            </Button>
            <Box style={{ marginTop: '20px', padding: '20px' }}>
              {chartData && (
                <Paper style={{ padding: '20px' }}>
                  <Typography variant="h6">График сравнения стоимости материалов от разных поставщиков</Typography>
                  <Bar data={chartData} />
                </Paper>
              )}
            </Box>
          </>
        )}
      </Container>
    </>
  );
};

export default PurchaserRequestView;
