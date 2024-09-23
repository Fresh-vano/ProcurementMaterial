// src/components/ReportGroup/GraphBuilder.js
import React, { useState, useEffect } from 'react';
import api from '../../api';
import { Grid, TextField, MenuItem, Paper, Button, Typography } from '@mui/material';
import SaveIcon from '@mui/icons-material/Save';
import { Bar, Pie, Line } from 'react-chartjs-2';
import FileDownload from 'js-file-download';

const entities = {
  Inventory: 'info',
  sf: 'sf'
};

const GraphBuilder = ({ onSaveChart }) => {
    const [selectedEntity, setSelectedEntity] = useState('');
    const [fields, setFields] = useState([]);
    const [selectedXField, setSelectedXField] = useState('');
    const [selectedYField, setSelectedYField] = useState('');
    const [chartType, setChartType] = useState('bar');
    const [chartTitle, setChartTitle] = useState('');
    const [chartColor, setChartColor] = useState('rgba(75,192,192,1)');
    const [yData, setYData] = useState([]);
  
    useEffect(() => {
      const fetchFields = async () => {
        if (selectedEntity) {
          const endpoint = `http://localhost:8080/FieldInfo/${entities[selectedEntity]}`;
          const response = await api.get(endpoint);
          setFields(response.data);
        }
      };
      fetchFields();
    }, [selectedEntity]);
  
    useEffect(() => {
      if (selectedYField) {
        const generateYData = () => {
          return ['January', 'February', 'March', 'April', 'May', 'June', 'July'].map(() =>
            Math.floor(Math.random() * 100)
          );
        };
        setYData(generateYData());
      }
    }, [selectedYField]);
  
    const handleEntityChange = (event) => {
      setSelectedEntity(event.target.value);
      setSelectedXField('');
      setSelectedYField('');
    };
  
    const handleXFieldChange = (event) => {
      setSelectedXField(event.target.value);
    };
  
    const handleYFieldChange = (event) => {
      setSelectedYField(event.target.value);
    };
  
    const handleChartTypeChange = (event) => {
      setChartType(event.target.value);
    };
  
    const handleSaveChart = () => {
      const newChart = {
        entity: selectedEntity,
        xField: selectedXField,
        yField: selectedYField,
        chartType,
        chartTitle,
        chartColor
      };
      onSaveChart(newChart);
      FileDownload(JSON.stringify(newChart, null, 2), 'chartConfig.json');
    };
  
    const renderChart = () => {
      const xData = ['Январь', 'Февраль', 'Март', 'Апрель', 'Май', 'Июнь', 'Июль'];
  
      const chartData = {
        labels: xData,
        datasets: [
          {
            label: selectedYField,
            backgroundColor: chartColor,
            borderColor: 'rgba(0,0,0,1)',
            borderWidth: 2,
            data: yData,
          },
        ],
      };
  
      switch (chartType) {
        case 'bar':
          return <Bar data={chartData} options={{ title: { display: true, text: chartTitle } }} />;
        case 'pie':
          return <Pie data={chartData} options={{ title: { display: true, text: chartTitle } }} />;
        case 'line':
          return <Line data={chartData} options={{ title: { display: true, text: chartTitle } }} />;
        default:
          return <Bar data={chartData} options={{ title: { display: true, text: chartTitle } }} />;
      }
    };

  return (
    <Paper style={{ padding: '20px' }}>
      <Typography variant="h6" style={{ paddingBottom: '20px' }}>
        Конструктор графиков
      </Typography>
      <Grid container spacing={3}>
        <Grid item xs={12} md={6}>
          <TextField
            select
            label="Выберите таблицу"
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
                label="Выберите параметр для оси X"
                value={selectedXField}
                onChange={handleXFieldChange}
                fullWidth
                style={{ marginTop: '20px' }}
              >
                {fields.map((field) => (
                  <MenuItem key={field} value={field}>
                    {field}
                  </MenuItem>
                ))}
              </TextField>
              <TextField
                select
                label="Выберите параметр для оси Y"
                value={selectedYField}
                onChange={handleYFieldChange}
                fullWidth
                style={{ marginTop: '20px' }}
              >
                {fields.map((field) => (
                  <MenuItem key={field} value={field}>
                    {field}
                  </MenuItem>
                ))}
              </TextField>
              <TextField
                label="Заголовок графика"
                value={chartTitle}
                onChange={(e) => setChartTitle(e.target.value)}
                fullWidth
                style={{ marginTop: '20px' }}
              />
            </>
          )}
        </Grid>
        <Grid item xs={12} md={6}>
          <TextField
            select
            label="Тип графика"
            value={chartType}
            onChange={handleChartTypeChange}
            fullWidth
          >
            <MenuItem value="bar">Столбчатая диаграмма</MenuItem>
            <MenuItem value="pie">Круговая диаграмма</MenuItem>
            <MenuItem value="line">Линейная диаграмма</MenuItem>
          </TextField>
          <Button
            variant="contained"
            color="primary"
            style={{ marginTop: '20px' }}
            onClick={handleSaveChart}
            startIcon={<SaveIcon />}
          >
            Сохранить
          </Button>
        </Grid>
        <Grid item xs={12}>
        {selectedXField && selectedYField && (
            <div style={{ height: '600px', width: '100%' }}>
            {renderChart()}
            </div>
        )}
        </Grid>
      </Grid>
    </Paper>
  );
};

export default GraphBuilder;
