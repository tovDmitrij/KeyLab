import { useNavigate } from "react-router-dom";

import { Box, Button, Container, Typography } from "@mui/material";

import Keyboard from "/src/assets/fullkb 1.png";

const Constructor = () => {

  const navigate = useNavigate();
  
  return (
    <Container
      maxWidth={false}
      id="constructor"
      sx={{
        backgroundColor: "#2D393B",
        alignItems: "center",
        textAlign: "center",
      }}
      disableGutters
    >
      <Typography
        fontSize={32}
        sx={{ fontWeight: "100", pt: "50px", color: "#FFFFFF" }}
      >
        Конструктор
      </Typography>
      <Button
        size="large"
        sx={{
          
          alignItems: "center",
          textAlign: "center",
          borderRadius: "30px",
          mt: "50px",
          mb: "50px",
          backgroundColor: "#FFFFFF",
          color: 'black',
          '&:hover': {
            //backgroundColor: '#FFFFFF',
            color: 'white',
          },
        }}
        variant="contained"
        onClick={() => navigate('/constructors')}
      >
        Перейти в режим конструктора
      </Button>
    </Container>
  );
};

export default Constructor;
