import React, { useEffect, useState } from "react";

import Header from "../../components/Header/Header";
import { Container, Grid, TextField, Typography } from "@mui/material";
import classes from "./Constructors.module.scss";
import KeycapsCard from "../../components/Card/KeycapsCard/KeycapsCard";
import BoxCard from "../../components/Card/BoxCard/BoxCard";
import SwitchCard from "../../components/Card/SwitchCard/SwitchCard";
import ModeEditOutlineIcon from "@mui/icons-material/ModeEditOutline";
import MoldaSetNameKeyboard from "../../components/Modals/MoldaSetNameKeyboard";
import { useDispatch } from "react-redux";
import { setTitle } from "../../store/keyboardSlice";
import { useAppSelector } from "../../store/redux";

const ConstructorsMain = () => {
  const { title } = useAppSelector(
    (state) => state.keyboardReduer
  );

  const [modal, setModal] = useState(false);
  const [titleKeyboard, setTitleKeyboard] = useState<string>(title);
  const dispatch = useDispatch();
  return (
    <>
      <Header />
      <Container maxWidth={"lg"} className={classes.container} disableGutters>
        <Typography
          fontSize={24}
          sx={{
            mt: "50px",
            display: "flex",
            justifyContent: "center",
            alignItems: "center",
            cursor: "pointer",
            "&:hover": {
              color: "grey",
            },
          }}
          onClick={() => setModal(true)}
        >
          {titleKeyboard} <ModeEditOutlineIcon sx={{ ml: "10px" }} />
        </Typography>
        <Grid
          container
          className={classes.list}
          justifyContent={"center"}
          spacing={0.5}
        >
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

      {modal && (
        <MoldaSetNameKeyboard
          open={modal}
          handleCloseModal={() => setModal(false)}
          onSubmitTitle={(title) => {
            setTitleKeyboard(title);
            dispatch(setTitle(title));
            setModal(false);
            
          }}
        />
      )}
    </>
  );
};

export default ConstructorsMain;
