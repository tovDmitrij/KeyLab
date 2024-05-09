import React, { useEffect, useState } from "react";

import Header from "../../components/Header/Header";
import { Container, Grid, Typography } from "@mui/material";
import classes from "./Constructors.module.scss";
import KeycapsCard from "../../components/Card/KeycapsCard/KeycapsCard";
import BoxCard from "../../components/Card/BoxCard/BoxCard";
import SwitchCard from "../../components/Card/SwitchCard/SwitchCard";

const ConstructorsMain = () => {
  return (
    <>
      <Header />
      <Container maxWidth={"lg"} className={classes.container} disableGutters>
        <Grid container className={classes.list} spacing={0.5}>
          <Grid item>
            <KeycapsCard />
          </Grid>
          <Grid item>
            <BoxCard />
          </Grid>
          <Grid item>
            <SwitchCard />
          </Grid>
        </Grid>
      </Container>
    </>
  );
};

export default ConstructorsMain;
