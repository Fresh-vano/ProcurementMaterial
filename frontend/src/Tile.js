// src/components/Tile.js
import React from 'react';
import { Box, Paper } from '@mui/material';

const Tile = ({ children }) => {
  return (
    <Paper style={{ padding: '20px', margin: '10px', minHeight:'594px' }}>
        {children}
    </Paper>
  );
};

export default Tile;
