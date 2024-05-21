import React, { useState, useEffect } from 'react';
import { AppBar, Toolbar, Typography, Button, Container, Paper, Grid, MenuItem, FormControl, InputLabel, Select, Box } from '@mui/material';
import { DataGrid } from '@mui/x-data-grid';
import { Bar, Pie, Line } from 'react-chartjs-2';
import 'chartjs-plugin-waterfall';
import axios from 'axios';
import { useAuth } from '../../utils/auth';
import { useNavigate } from 'react-router-dom';

const MAX_ITEMS = 5; // Максимальное количество элементов для отображения в MenuItem

const PurchaserRequestView = () => {
  const { logout } = useAuth();
  const navigate = useNavigate();
  const [selectedMaterial, setSelectedMaterial] = useState('');
  const [selectedInns, setSelectedInns] = useState([]);
  const [materials, setMaterials] = useState([]);
  const [chartsData, setChartsData] = useState({ bar: null, bar1: null, bar2: null, waterfall: null });
  const [innOptions, setInnOptions] = useState([]);
  const [materialColumns, setMaterialColumns] = useState([]);
  const [materialRows, setMaterialRows] = useState([]);
  const [sfColumns, setSfColumns] = useState([]);
  const [sfRows, setSfRows] = useState([]);

  useEffect(() => {
    const fetchMaterials = async () => {
      try {
        const res = await axios.get('http://localhost:8080/chart/GetUniqueMaterials');
        console.log(res)
        setMaterials(res.data);
      } catch (error) {
        console.error('Error fetching materials:', error);
      }
    };

    fetchMaterials();
  }, []);

  const handleMaterialChange = async (event) => {
    const material = event.target.value;
    setSelectedMaterial(material);
    setSelectedInns([]); // Reset selected INNs when material changes

      try {
        const innResponse = await axios.get(`http://localhost:8080/chart/GetUniqueINNsByMaterial?request=${material}`);
        console.log(innResponse)
        setInnOptions(innResponse.data);
      } catch (error) {
        console.error('Error fetching inn:', error);
      }

    try {
      const materialResponse = await axios.get('http://localhost:8080/table/material');
      const sfResponse = await axios.get('http://localhost:8080/table/sf');
      setMaterialColumns(materialResponse.data.columns);
      setMaterialRows(materialResponse.data.rows);
      setSfColumns(sfResponse.data.columns);
      setSfRows(sfResponse.data.rows);
    } catch (error) {
      console.error('Error fetching table data:', error);
    }
  };

  const handleInnChange = (event) => {
    setSelectedInns(event.target.value);
  };

  const handleLogout = () => {
    logout(() => navigate('/login'));
  };

  const generateChartsData = async () => {
    try {
      const costComparisonResponse = await axios.post('http://localhost:8080/chart/4GenerateSupplierCostComparisonChartJson', {
        material: selectedMaterial,
        INNs: selectedInns
      });
  
      const costOverTimeResponse = await axios.post('http://localhost:8080/chart/3GenerateCostOverTimeChartJson', {
        material: selectedMaterial
      });
  
      const quantityComparisonResponse = await axios.post('http://localhost:8080/chart/7GenerateSupplierQuantityComparisonChartJson', {
        material: selectedMaterial
      });
      
      const performanceResponse = await axios.post('http://localhost:8080/chart/GeneratePerformanceChartJson', {
        material: selectedMaterial,
        INNs: selectedInns
      });
debugger
      setChartsData({
        bar: costComparisonResponse.data.bar,
        bar1: costOverTimeResponse.data.bar,
        bar2: quantityComparisonResponse.data.bar,
        waterfall: performanceResponse.data.chart
      });
    } catch (error) {
      console.error('Error fetching chart data:', error);
    }
    console.log(chartsData)
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
        <Box sx={{ display: 'flex', justifyContent: 'center', alignItems: 'center', mt: 3 }}>
          <Paper sx={{ p: 2, width: '100%' }}>
          <FormControl fullWidth>
            <Typography id="material-select-label">Выберите материал</Typography>
            <Select
              labelId="material-select-label"
              id="material-select"
              value={selectedMaterial}
              onChange={handleMaterialChange}
              MenuProps={{
                PaperProps: {
                  style: {
                    maxHeight: 200, // Задание maxHeight для всплывающего меню
                  },
                },
              }}
            >
              {materials.map((material) => (
                <MenuItem key={material} value={material} disabled={materials.length >= 3 && !materials.includes(material)}>
                  {material}
                </MenuItem>
              ))}
            </Select>
          </FormControl>
            {selectedMaterial && (
              <>
                <FormControl fullWidth sx={{ mt: 2 }}>
                  <Typography id="inn-select-label">Выберите ИНН</Typography>
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
                <Button variant="contained" color="primary" onClick={generateChartsData} sx={{ mt: 2 }}>
                  Обновить график
                </Button>
              </>
            )}
          </Paper>
        </Box>
        {selectedMaterial && (
          <>
            <Box sx={{ mt: 3 }}>
              <Grid container spacing={3}>
                <Grid item xs={12}>
                  <Paper sx={{ height: 400, width: '100%' }}>
                    <DataGrid rows={materialRows} columns={materialColumns} pageSize={5}/>
                  </Paper>
                </Grid>
                <Grid item xs={12}>
                  <Paper sx={{ height: 400, width: '100%' }}>
                    <DataGrid rows={sfRows} columns={sfColumns} pageSize={5}/>
                  </Paper>
                </Grid>
              </Grid>
            </Box>
            <Box sx={{ mt: 3 }}>
              <Grid container spacing={3}>
                <Grid item xs={12} md={6}>
                  <Paper sx={{ p: 2 }}>
                    {chartsData.bar && 
                    <>
                    <Typography variant="h6" style={{ flexGrow: 1 }}>
                      Стоимость материалов от разных поставщиков
                    </Typography>
                      <Bar data={chartsData.bar} />
                    </>
                    }
                  </Paper>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Paper sx={{ p: 2 }}>
                    {chartsData.bar1 && 
                    <>
                      <Typography variant="h6" style={{ flexGrow: 1 }}>
                        График изменения стоимости в зависимости от даты проводки
                      </Typography>
                      <Bar data={chartsData.bar1} />
                    </>
                    }
                  </Paper>
                </Grid>
                <Grid item xs={12}  md={6}>
                  <Paper sx={{ p: 2 }}>
                    {chartsData.bar2 && 
                    <>
                      <Typography variant="h6" style={{ flexGrow: 1 }}>
                        Количество материалов от разных поставщиков
                      </Typography>
                      <Bar data={chartsData.bar2} />
                    </>
                    }
                  </Paper>
                </Grid>
                <Grid item xs={12} md={6}>
                  <Paper sx={{ p: 2 }}>
                    {chartsData.waterfall && 
                    <>
                      <Typography variant="h6" style={{ flexGrow: 1 }}>
                        Соотношение поступления к фактическому запасу за период
                      </Typography>
                      <Bar data={chartsData.waterfall} />
                    </>
                    }
                  </Paper>
                </Grid>
              </Grid>
            </Box>
          </>
        )}
      </Container>
    </>
  );
};

export default PurchaserRequestView;
