// src/components/ReportGroup/ChartBuilder.js
import React, { useState } from 'react';
import { Container, Typography, Grid, Paper, AppBar, Toolbar, Button, IconButton } from '@mui/material';
import { useAuth } from '../../utils/auth';
import { useNavigate } from 'react-router-dom';
import DeleteIcon from '@mui/icons-material/Delete';
import FileUpload from './FileUpload';
import GraphBuilder from './GraphBuilder';
import LogoutButton from '../Auth/LogoutButton';

const ChartBuilder = () => {
  const [savedCharts, setSavedCharts] = useState([]);
  const { logout } = useAuth();
  const navigate = useNavigate();

  const handleSaveChart = (newChart) => {
    setSavedCharts([...savedCharts, newChart]);
  };

  const handleDeleteChart = (index) => {
    const updatedCharts = savedCharts.filter((_, i) => i !== index);
    setSavedCharts(updatedCharts);
  };

  return (
    <>
      <AppBar position="static">
        <Toolbar>
          <Typography variant="h6" style={{ flexGrow: 1 }}>
            Страница группы отчета
          </Typography>
          <LogoutButton/>
        </Toolbar>
      </AppBar>
      <Container>
        <Grid container spacing={3} style={{ marginTop: '20px' }}>

          <Grid item xs={12}>
    <Paper style={{ padding: '20px' }}>
            <FileUpload />
          </Paper>
          </Grid>
          <Grid item xs={12}>
            <GraphBuilder onSaveChart={handleSaveChart} />
          </Grid>
        </Grid>
      </Container>
    </>
  );
};

export default ChartBuilder;
