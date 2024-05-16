import { Box, Container, Typography } from "@mui/material";

import SchemePic from "/src/assets/scheme.png";

const Scheme = () => {

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
        sx={{ fontWeight: "100", pt: "50px", color: "#FFFFFF" }}
      >
        Схема клавиатуры
      </Typography>
      <div style={{ textAlign: "center", marginTop: "50px" }}>
        <Box
          component="img"
          sx={{ width: "60%", mb: "50px" }}
          src={SchemePic}
          draggable="false"
          loading="lazy"
        />
      </div>
    </Container>
  );
};

export default Scheme;
