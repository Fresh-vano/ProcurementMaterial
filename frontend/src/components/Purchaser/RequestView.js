// src/components/Purchaser/RequestView.js
import React from 'react';
import { Container, Typography, Grid, Paper, Button } from '@mui/material';
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
import { useAuth } from '../../utils/auth';
import { useNavigate } from 'react-router-dom';

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

const sampleRequest = {
  id: 1,
  item: 'Office Chairs',
  quantity: 50,
  currentStock: 30,
  price: 150,
  databasePrice: 140,
};

const PurchaserRequestView = () => {
    const { logout } = useAuth();
    const navigate = useNavigate();
  
    const handleLogout = () => {
      logout(() => navigate('/login'));
    };

  return (
    <Container>
      <Typography variant="h4">Purchaser Request View</Typography>
      <Paper style={{ padding: '20px', marginTop: '20px' }}>
        <Typography variant="h6">Request ID: {sampleRequest.id}</Typography>
        <Typography>Item: {sampleRequest.item}</Typography>
        <Typography>Quantity: {sampleRequest.quantity}</Typography>
        <Typography>Current Stock: {sampleRequest.currentStock}</Typography>
        <Typography>Price: ${sampleRequest.price}</Typography>
        <Typography>Database Price: ${sampleRequest.databasePrice}</Typography>
      </Paper>
      <Button variant="contained" color="secondary" onClick={handleLogout}>
        Logout
      </Button>
      <Grid container spacing={3} style={{ marginTop: '20px' }}>
        <Grid item xs={12} md={6}>
          <Paper style={{ padding: '20px' }}>
            <Typography variant="h6">Stock Comparison</Typography>
            <Typography>Requested Quantity: {sampleRequest.quantity}</Typography>
            <Typography>Current Stock: {sampleRequest.currentStock}</Typography>
          </Paper>
        </Grid>
        <Grid item xs={12} md={6}>
          <Paper style={{ padding: '20px' }}>
            <Typography variant="h6">Price Comparison</Typography>
            <Typography>Offered Price: ${sampleRequest.price}</Typography>
            <Typography>Database Price: ${sampleRequest.databasePrice}</Typography>
          </Paper>
        </Grid>
      </Grid>
    </Container>
  );
};

export default PurchaserRequestView;
