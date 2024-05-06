import { useNavigate } from "react-router-dom";

import { Box, Container, Grid, Typography } from "@mui/material";
import PreviewCard from "../Card/PreviewCard/PreviewCard";

const Models = () => {
  return (
    <Container
      id="models"
      maxWidth={false}
      sx={{ width: "86%"}}
      disableGutters
    >
      <Typography fontSize={32} sx={{ textAlign: "center", pt: "50px" }}>
        3D - модели
      </Typography>
      <Grid container spacing={5}>
        <Grid item>
          <PreviewCard />
        </Grid>
      </Grid>
    </Container>
  );
};

export default Models;
