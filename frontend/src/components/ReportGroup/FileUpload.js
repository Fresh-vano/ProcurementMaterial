import React, { useState } from 'react';
import axios from 'axios';
import { Button, Snackbar, Alert, Checkbox, FormControlLabel, TextField, Box, CircularProgress, Typography } from '@mui/material';

const FileUpload = () => {
  const [file, setFile] = useState(null);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);
  const [open, setOpen] = useState(false);
  const [isInventoryFile, setIsInventoryFile] = useState(false);
  const [loading, setLoading] = useState(false);

  const handleFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  const handleCheckboxChange = (event) => {
    setIsInventoryFile(event.target.checked);
  };

  const handleUpload = async () => {
    if (!file) {
      setError('No file selected');
      setOpen(true);
      return;
    }

    const formData = new FormData();
    formData.append('file', file);

    const url = isInventoryFile ? 'http://localhost:8080/File' : 'http://localhost:8080/File/sf';

    setLoading(true);
    try {
      await axios.post(url, formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      setSuccess('File uploaded successfully!');
      setOpen(true);
    } catch (error) {
      setError('Error uploading file');
      setOpen(true);
    } finally {
      setLoading(false);
    }
  };

  const handleClose = () => {
    setError(null);
    setSuccess(null);
    setOpen(false);
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
        <Button variant="contained" color="primary" onClick={handleUpload} disabled={loading}>
          {loading ? <CircularProgress size={24} /> : 'Загрузить'}
        </Button>
      </Box>
      <Snackbar open={open} autoHideDuration={6000} onClose={handleClose}>
        {error ? (
          <Alert onClose={handleClose} severity="error" sx={{ width: '100%' }}>
            {error}
          </Alert>
        ) : null}
        {success ? (
          <Alert onClose={handleClose} severity="success" sx={{ width: '100%' }}>
            {success}
          </Alert>
        ) : null}
      </Snackbar>
    </Box>
  );
};

export default FileUpload;
