import React, { useState } from 'react';
import api from '../../api';
import { Button, Snackbar, Alert, Checkbox, FormControlLabel, TextField, Box, CircularProgress, Typography } from '@mui/material';

const FileUpload = () => {
  const [file, setFile] = useState(null);
  const [error, setError] = useState('');
  const [success, setSuccess] = useState('');
  const [openSuccess, setOpenSuccess] = useState(false);
  const [openError, setOpenError] = useState(false);
  const [isInventoryFile, setIsInventoryFile] = useState(false);
  const [loading, setLoading] = useState(false);
  const [date, setDate] = useState('');

  const handleFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  const handleCheckboxChange = (event) => {
    setIsInventoryFile(event.target.checked);
  };

  const handleDateChange = (event) => {
    setDate(event.target.value);
  };

  const handleUpload = async () => {
    if (!file) {
      setError('No file selected');
      setOpenError(true);
      return;
    }

    const formData = new FormData();
    formData.append('files', file);
    if (isInventoryFile) {
      formData.append('dateString', date);
    }
    
    // Определение URL эндпоинта
    const url = isInventoryFile ? '/File' : '/File/sf'; // Используйте относительные пути    

    setLoading(true);
    try {
      const response = await api.post(url, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });

      if (response.status === 200) {
        setSuccess('File uploaded successfully!');
        setOpenSuccess(true);
      } else {
        setError('Error uploading file');
        setOpenError(true);
      }
    } catch (error) {
      setError('Error uploading file');
      setOpenError(true);
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    setError(null);
    setSuccess(null);
    setOpenSuccess(false);
    setOpenError(false);
  };

  return (
    <Box sx={{ mt: 3 }}>
      <Typography variant="h6">Загрузка файлов</Typography>
      <Box component="form" noValidate autoComplete="off" sx={{ mt: 2 }}>
        <TextField
          type="file"
          onChange={handleFileChange}
          fullWidth
          sx={{ mb: 2 }}
        />
        <FormControlLabel
          control={
            <Checkbox checked={isInventoryFile} onChange={handleCheckboxChange} color="primary" />
          }
          label="Файл материалов"
        />
        {isInventoryFile && (
          <TextField
            type="date"
            value={date}
            onChange={handleDateChange}
            fullWidth
            sx={{ mb: 2 }}
          />
        )}
        <Button variant="contained" color="primary" onClick={handleUpload} disabled={loading}>
          {loading ? <CircularProgress size={24} /> : 'Загрузить'}
        </Button>
      </Box>
      <Snackbar open={openSuccess} autoHideDuration={6000} onClose={handleClose}>
          <Alert onClose={handleClose} severity="success" sx={{ width: '100%' }}>
            {success}
          </Alert>
      </Snackbar>
      <Snackbar open={openError} autoHideDuration={6000} onClose={handleClose}>
          <Alert onClose={handleClose} severity="error" sx={{ width: '100%' }}>
            {error}
          </Alert>
      </Snackbar>
    </Box>
  );
};

export default FileUpload;
