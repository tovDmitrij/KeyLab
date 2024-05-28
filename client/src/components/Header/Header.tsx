import React, { useEffect, useState } from "react";

import AppBar from "@mui/material/AppBar";
import Toolbar from "@mui/material/Toolbar";
import Container from "@mui/material/Container";
import KeyboardIcon from "../Icons/IconLogo/IconKeylab";
import { Box, Typography } from "@mui/material";

import classes from "./Header.module.scss";
import { useAppSelector } from "../../store/redux";

import { RootState } from "../../store/store";
import { Link, useNavigate } from "react-router-dom";
import { resetState } from "../../store/profileSlice";
import { logOut } from "../../store/authSlice";
import { useDispatch } from "react-redux";

const Header = () => {
  const [title, setTitle] = useState<string | undefined>();
  const currentUrl = window.location.href;
  const navigate = useNavigate();
  const dispatch = useDispatch();
  const { nickName, isAdmin } = useAppSelector(
    (state: RootState) => state.profileReducer
  );

  useEffect(() => {
    if (currentUrl.includes("/constrSwitch")) {
      setTitle("Выбор свитчей");
    } else if (currentUrl.includes("/constrBoxes")) {
      setTitle("Выбор размера клавиатуры");
    } else if (currentUrl.includes("/constrKeys")) {
      setTitle("Выбор кейкапов");
    } else if (currentUrl.includes("/constructors")) {
      setTitle("Комплектующие");
    } else if (currentUrl.includes("/constrKeyboard")) {
      setTitle("3D просмотр");
    } else if (currentUrl.includes("/stats")) {
      setTitle("Статистика");
    } else if (currentUrl.includes("/keyboard/")) {
      setTitle("3D просмотр");
    } 
  }, [currentUrl]);

  const handleClickExit = () => {
    dispatch(resetState());
    dispatch(logOut());
  };

  const getNickName = () => {
    return nickName || localStorage.getItem('nickName')
  }

  const getAdmin = () => {
    return isAdmin || localStorage.getItem('isAdmin') === "true"  
  }

  const scrollToSection = (sectionId: string) => {
    const sectionElement = document.getElementById(sectionId);
    const offset = 128;
    if (sectionElement) {
      const targetScroll = sectionElement.offsetTop - offset;
      sectionElement.scrollIntoView({ behavior: 'smooth' });
      window.scrollTo({
        top: targetScroll,
        behavior: 'smooth',
      });

    }
  };


  return (
    <AppBar position="absolute">
      <Container maxWidth={false}>
        {location.pathname.includes("/register") ||
        location.pathname.includes("/login") ? (
          <Toolbar className={classes.header_log_reg} disableGutters>
              <div className={classes.logo} onClick={() => navigate("/")}>
                <KeyboardIcon sx={{ fontSize: 50 }} />
                <Typography className={classes.link} fontSize={34}>
                  Keylab
                </Typography>
              </div>
          </Toolbar>
        ) : (
          <Toolbar disableGutters>
            <Box component="div" className={classes.header_left}>
              <div className={classes.logo} onClick={() => navigate("/")}>
                <KeyboardIcon sx={{ fontSize: 50 }} />
                <Typography className={classes.link} fontSize={34}>
                  Keylab
                </Typography>
              </div>
              {!title && (
                <>
                  <Typography
                    className={classes.link}
                    fontSize={14}
                    sx={{ marginLeft: 8 }}
                    onClick={() => scrollToSection('about')}
                  >
                    О нас
                  </Typography>
                  <Typography
                    className={classes.link}
                    fontSize={14}
                    sx={{ marginLeft: 8 }}
                    onClick={() => scrollToSection('models')}
                  >
                    3D - модели
                  </Typography>
                  <Typography
                    className={classes.link}
                    fontSize={14}
                    sx={{ marginLeft: 8 }}
                    onClick={() => scrollToSection('constructor')}
                  >
                    Конструктор
                  </Typography>
                  {getAdmin() && (
                    <Typography
                      className={classes.link}
                      fontSize={14}
                      sx={{ marginLeft: 8 }}
                      onClick={() => navigate('/stats')}
                    >
                      Статистика
                    </Typography>
                  )}
                </>
              )}
            </Box>
            {title && (
              <Box component="div" className={classes.header_right}>
                <Typography className={classes.link} fontSize={14}>
                  {title}
                </Typography>
              </Box>
            )}
            <Box component="div" className={classes.header_right}>
              <Typography
                className={classes.link}
                fontSize={14}
                sx={{ marginLeft: 8, color: "#0094ff" }}
              >
                {getNickName()}
              </Typography>
              <Typography
                className={classes.link}
                fontSize={14}
                sx={{ marginLeft: 2 }}
                onClick={() => {
                  localStorage.getItem("token") ? handleClickExit() : navigate("/login");
                }}
              >
                {localStorage.getItem("token") ? 'Выйти' : 'Войти'}
              </Typography>
            </Box>
          </Toolbar>
        )}
      </Container>
    </AppBar>
  );
};
export default Header;
