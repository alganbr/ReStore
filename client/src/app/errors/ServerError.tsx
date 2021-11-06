import {Button, Container, Divider, Paper, Typography } from '@mui/material';
import React from 'react';
import { Link, useLocation } from 'react-router-dom';

const ServerError = () => {
  const {state} = useLocation<any>();
  
  return (
    <Container component={Paper}>
      {state?.error ? (
        <>
          <Typography variant='h3' color='error' gutterBottom>{state.error.title}</Typography>
          <Divider />
          <Typography>{state.error.detail || 'Internal server error'}</Typography>
        </>
      ) : (
        <Typography variant='h5'> Server Error</Typography>
      )}
      <Button component={Link} to={'/catalog'}>Go back to shop</Button>
    </Container>
  );
};

export default ServerError;
