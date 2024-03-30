import * as React from "react";

import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Container from "@mui/material/Container";
import KeyboardIcon from "./IconKeylab";
import { Box, Typography } from "@mui/material";

import classes from "./Header.module.scss";
import { useAppSelector } from "../../store/redux";

import { RootState } from "../../store/store";


const Header = () => {

  const {nickName} = useAppSelector((state : RootState) => state.profileReducer)

  const handleClickExit = () => {
    
  }

  return (
    <AppBar position="absolute">
      <Container maxWidth={false}>
        {location.pathname.includes("/register") ||
        location.pathname.includes("/login") ? (
          <Toolbar className={classes.header_log_reg} disableGutters>
            <KeyboardIcon sx={{ fontSize: 50 }} />
            <Typography fontSize={34}> Keylab </Typography>
          </Toolbar>
        ) : (
          <Toolbar disableGutters>
            <Box component="div" className={classes.header_left}>
              <KeyboardIcon sx={{ fontSize: 50 }} />
              <Typography fontSize={34}> Keylab </Typography>
              <Typography fontSize={14} sx={{ marginLeft: 8 }}> О нас </Typography>
              <Typography fontSize={14} sx={{ marginLeft: 8 }}> 3D - модели </Typography>
              <Typography fontSize={14} sx={{ marginLeft: 8 }}> Конструктор </Typography>
            </Box>
            <Box component="div" className={classes.header_right}>
              <Typography fontSize={14} sx={{ marginLeft: 8 }}> {nickName} </Typography>
              <Typography fontSize={14} sx={{ marginLeft: 8 }} onClick={handleClickExit}> Выйти </Typography>
            </Box>
          </Toolbar>
        )}
      </Container>
    </AppBar>
  );
};
export default Header;
