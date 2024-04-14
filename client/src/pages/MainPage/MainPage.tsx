import React, { useEffect, useState } from "react";
import Header from "../../components/Header/Header";
import { Box, Container, Grid, Typography } from "@mui/material";

import classes from "./MainPage.module.scss";
import { useGetAuthKeyboardsQuery } from "../../services/keyboardService";
import PreviewCard from "../../components/Card/PreviewCard/PreviewCard";


const MainPage = () => {
  const { data } = useGetAuthKeyboardsQuery({
    page: 1,
    pageSize: 10,
  });

  console.log(data);

  return (
    <>
      <Header />
        <Container maxWidth='lg' className={classes.container} disableGutters>
          <Typography fontSize={32} sx={{ textAlign: "center" }}>
            3D - модели
          </Typography>
          <Grid container spacing={5}>
            <Grid item>
              <PreviewCard />
            </Grid>
          </Grid>
        </Container>
    </>
  );
};

export default MainPage;
