import React, {useState} from 'react';
import { Container, createTheme, CssBaseline, ThemeProvider } from '@mui/material';
import Catalog from '../../features/catalog/Catalog';
import Header from './Header';
import { Route } from 'react-router-dom';
import HomePage from '../../features/home/HomePage';
import ProductDetail from '../../features/catalog/ProductDetail';
import AboutPage from '../../features/about/AboutPage';
import ContactPage from '../../features/contact/ContactPage';

const App = () => {
  const [darkMode, setDarkMode] = useState(false);
  const paletteType = darkMode ? 'dark' : 'light';

  const theme = createTheme({
    palette: {
      mode: paletteType,
      background: {
        default: darkMode ? '#121212' : '#eaeaea'
      }
    }
  })
  
  const handleThemeChange = () => setDarkMode(!darkMode);
  
  return (
    <ThemeProvider theme={theme}>
      <CssBaseline />
      <Header darkMode={darkMode} handleThemeChange={handleThemeChange} />
      <Container>
        <Route exact path='/' component={HomePage} />
        <Route exact path='/catalog' component={Catalog} />
        <Route path='/catalog/:id' component={ProductDetail} />
        <Route path='/about' component={AboutPage} />
        <Route path='/contact' component={ContactPage} />
      </Container>
    </ThemeProvider>
  );
}

export default App;
