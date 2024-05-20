// src/components/ReportGroup/ChartBuilder.js
import React, { useState } from 'react';
import { Container, Typography, Grid, Paper, TextField, MenuItem, Button, IconButton } from '@mui/material';
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
import { Bar, Pie, Line } from 'react-chartjs-2';
import { useAuth } from '../../utils/auth';
import { useNavigate } from 'react-router-dom';
import SaveIcon from '@mui/icons-material/Save';
import DeleteIcon from '@mui/icons-material/Delete';

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

const entities = {
  Products: ['Name', 'Price', 'Stock', 'Category'],
  Orders: ['OrderID', 'CustomerID', 'Amount', 'Date'],
  Customers: ['CustomerID', 'Name', 'Country', 'Age'],
};

const operations = ['+', '-', '*', '/'];

const ChartBuilder = () => {
  const [selectedEntity, setSelectedEntity] = useState('');
  const [selectedXField, setSelectedXField] = useState('');
  const [selectedYField, setSelectedYField] = useState('');
  const [operation, setOperation] = useState('');
  const [customValue, setCustomValue] = useState('');
  const [chartType, setChartType] = useState('bar');
  const [savedCharts, setSavedCharts] = useState([]);
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleEntityChange = (event) => {
    setSelectedEntity(event.target.value);
    setSelectedXField('');
    setSelectedYField('');
    setOperation('');
    setCustomValue('');
  };

  const handleXFieldChange = (event) => {
    setSelectedXField(event.target.value);
  };

  const handleYFieldChange = (event) => {
    setSelectedYField(event.target.value);
  };

  const handleOperationChange = (event) => {
    setOperation(event.target.value);
  };

  const handleCustomValueChange = (event) => {
    setCustomValue(event.target.value);
  };

  const handleChartTypeChange = (event) => {
    setChartType(event.target.value);
  };

  const handleLogout = () => {
    logout(() => navigate('/login'));
  };

  const handleSaveChart = () => {
    const newChart = {
      entity: selectedEntity,
      xField: selectedXField,
      yField: selectedYField,
      operation,
      customValue,
      chartType,
    };
    setSavedCharts([...savedCharts, newChart]);
  };

  const handleDeleteChart = (index) => {
    const updatedCharts = savedCharts.filter((_, i) => i !== index);
    setSavedCharts(updatedCharts);
  };

  const renderChart = () => {
    const xData = ['January', 'February', 'March', 'April', 'May', 'June', 'July'];
    const yData = xData.map(() => Math.floor(Math.random() * 100));

    const chartData = {
      labels: xData,
      datasets: [
        {
          label: `${selectedYField} ${operation} ${customValue}`,
          backgroundColor: 'rgba(75,192,192,1)',
          borderColor: 'rgba(0,0,0,1)',
          borderWidth: 2,
          data: yData,
        },
      ],
    };

    switch (chartType) {
      case 'bar':
        return <Bar data={chartData} />;
      case 'pie':
        return <Pie data={chartData} />;
      case 'line':
        return <Line data={chartData} />;
      default:
        return <Bar data={chartData} />;
    }
  };

  return (
    <Container>
      <Typography variant="h4">Report Group Chart Builder</Typography>
      <Button variant="contained" color="secondary" onClick={handleLogout}>
        Logout
      </Button>
      <Grid container spacing={3} style={{ marginTop: '20px' }}>
        <Grid item xs={12} md={6}>
          <Paper style={{ padding: '20px' }}>
            <TextField
              select
              label="Select Entity"
              value={selectedEntity}
              onChange={handleEntityChange}
              fullWidth
            >
              {Object.keys(entities).map((entity) => (
                <MenuItem key={entity} value={entity}>
                  {entity}
                </MenuItem>
              ))}
            </TextField>
            {selectedEntity && (
              <>
                <TextField
                  select
                  label="Select X Field"
                  value={selectedXField}
                  onChange={handleXFieldChange}
                  fullWidth
                  style={{ marginTop: '20px' }}
                >
                  {entities[selectedEntity].map((field) => (
                    <MenuItem key={field} value={field}>
                      {field}
                    </MenuItem>
                  ))}
                </TextField>
                <TextField
                  select
                  label="Select Y Field"
                  value={selectedYField}
                  onChange={handleYFieldChange}
                  fullWidth
                  style={{ marginTop: '20px' }}
                >
                  {entities[selectedEntity].map((field) => (
                    <MenuItem key={field} value={field}>
                      {field}
                    </MenuItem>
                  ))}
                </TextField>
                <TextField
                  select
                  label="Operation"
                  value={operation}
                  onChange={handleOperationChange}
                  fullWidth
                  style={{ marginTop: '20px' }}
                >
                  {operations.map((op) => (
                    <MenuItem key={op} value={op}>
                      {op}
                    </MenuItem>
                  ))}
                </TextField>
                <TextField
                  label="Custom Value"
                  value={customValue}
                  onChange={handleCustomValueChange}
                  fullWidth
                  style={{ marginTop: '20px' }}
                />
              </>
            )}
          </Paper>
        </Grid>
        <Grid item xs={12} md={6}>
          <Paper style={{ padding: '20px' }}>
            <TextField
              select
              label="Chart Type"
              value={chartType}
              onChange={handleChartTypeChange}
              fullWidth
            >
              <MenuItem value="bar">Bar</MenuItem>
              <MenuItem value="pie">Pie</MenuItem>
              <MenuItem value="line">Line</MenuItem>
            </TextField>
            <Button
              variant="contained"
              color="primary"
              style={{ marginTop: '20px' }}
              onClick={handleSaveChart}
              startIcon={<SaveIcon />}
            >
              Save Chart
            </Button>
          </Paper>
        </Grid>
        <Grid item xs={12}>
          <Paper style={{ padding: '20px' }}>{selectedXField && selectedYField && renderChart()}</Paper>
        </Grid>
        <Grid item xs={12}>
          <Typography variant="h5">Saved Charts</Typography>
          {savedCharts.map((chart, index) => (
            <Paper key={index} style={{ padding: '20px', marginTop: '20px' }}>
              <Typography>{`${chart.entity}: ${chart.xField} ${chart.operation} ${chart.customValue}`}</Typography>
              <IconButton onClick={() => handleDeleteChart(index)}>
                <DeleteIcon />
              </IconButton>
              {/* You can add more details about the chart and a button to load/render the saved chart */}
            </Paper>
          ))}
        </Grid>
      </Grid>
    </Container>
  );
};

export default ChartBuilder;
