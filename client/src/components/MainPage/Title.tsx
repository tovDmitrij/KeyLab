import { useNavigate } from "react-router-dom";

import { Box, Button, Container, Typography } from "@mui/material";

import Keyboard from "/src/assets/fullkb 1.png";

const Title = () => {

  const navigate = useNavigate();
  
  return (
    <Container
      maxWidth={false}
      sx={{
        backgroundColor: "#2D393B",
        alignItems: "center",
        textAlign: "center",
      }}
      disableGutters
    >
      <Typography
        fontSize={32}
        sx={{ fontWeight: "100", pt: "100px", color: "#FFFFFF" }}
      >
        Собери свою клавиатуру и послушай
      </Typography>
      <Button
        size="small"
        sx={{
          width: "200px",
          alignItems: "center",
          textAlign: "center",
          borderRadius: "30px",
          mt: "50px"
        }}
        variant="contained"
        onClick={() => navigate('/login')}
      >
        Cобрать
      </Button>
      <div style={{ textAlign: "center", marginTop: "50px" }}>
        <Box component="img" sx={{ width: "50%", mb: "50px" }} src={Keyboard} />
      </div>
    </Container>
  );
};

export default Title;
