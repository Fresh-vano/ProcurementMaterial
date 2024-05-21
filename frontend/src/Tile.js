// src/components/Tile.js
import React from 'react';
import { Paper } from '@mui/material';

const Tile = ({ children }) => {
  return (
    <Paper style={{ padding: '20px', margin: '20px 0' }}>
      {children}
    </Paper>
  );
};

export default Tile;
