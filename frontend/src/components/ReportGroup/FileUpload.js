// src/components/ReportGroup/FileUpload.js
import React, { useState } from 'react';
import axios from 'axios';
import { Button, Snackbar, Alert } from '@mui/material';

const FileUpload = () => {
  const [file, setFile] = useState(null);
  const [error, setError] = useState(null);
  const [success, setSuccess] = useState(null);
  const [open, setOpen] = useState(false);

  const handleFileChange = (event) => {
    setFile(event.target.files[0]);
  };

  const handleUpload = async () => {
    if (!file) {
      setError('No file selected');
      setOpen(true);
      return;
    }

    const formData = new FormData();
    formData.append('file', file);

    try {
      await axios.post('http://localhost:8080/File', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
          'Access-Control-Allow-Origin': '*'
        },
      });
      setSuccess('Загружено успешно!');
      setOpen(true);
    } catch (error) {
      setError('Error uploading file');
      setOpen(true);
    }
  };

  const handleClose = () => {
    setError(null);
    setOpen(false);
  };

  return (
    <>
      <input type="file" onChange={handleFileChange} />
      <Button variant="contained" color="primary" onClick={handleUpload}>
        Upload
      </Button>
      <Snackbar open={open} autoHideDuration={6000} onClose={handleClose}>
        {error ? 
        <Alert onClose={handleClose} severity="error" sx={{ width: '100%' }}>
          {error}
        </Alert>
        :
        <></>
        }
        {success ? 
        <Alert onClose={handleClose} severity="success" sx={{ width: '100%' }}>
          {success}
        </Alert>
        :
        <></>
        }
      </Snackbar>
    </>
  );
};

export default FileUpload;
