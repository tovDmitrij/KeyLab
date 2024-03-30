import React, { useEffect, useRef, useState } from "react";
import { Link, useNavigate } from "react-router-dom";

import Button from "@mui/material/Button";
import {
  Alert,
  Box,
  Container,
  Snackbar,
  TextField,
  Typography,
} from "@mui/material";

import { useAppDispatch } from "../../store/redux";
import { singnIn } from "../../store/authSlice";
import { useLoginMutation } from "../../services/authSetvice";
import Header from "../../components/Header/Header";

import classes from "./Login.module.scss";

/**
 * Тип данных представляет информацию о пользователе для авторизации.
 */
type TUser = {
  /**
   * Пароль пользователя.
   */
  password: string | null;
  /**
   * Email
   */
  email: string | null;
};

const Login = () => {
  const [open, setOpen] = useState<boolean>(false);

  const [errorText, setErrorText] = useState<string>();
  const [isError, setIsError] = useState<boolean>(false);
  const [user, setUser] = useState<TUser>({
    password: null,
    email: null,
  });

  const [login] = useLoginMutation();
  const dispatch = useAppDispatch();
  const navigate = useNavigate();

  const validateUser = (): boolean => {
    return !!(user.password && user.email);
  };

  const handleChange = (argName: string, argValue: string) => {
    setUser({
      ...user,
      [argName]: argValue,
    });
  };

  const handleClick = (event: React.MouseEvent<Element, MouseEvent>) => {
    event.preventDefault();
    if (validateUser()) {
      login(user as TUser)
        .unwrap()
        .then((data) => {
          dispatch(singnIn(data));
          navigate("/MainPage");
        })
        .catch((error) => {
          setIsError(true);
          setErrorText(error.data);
        });
    }
  };

  const handleClose = (
    event: React.SyntheticEvent | Event,
    reason?: string
  ) => {
    if (reason === "clickaway") {
      return;
    }

    setIsError(false);
  };

  useEffect(() => {
    setOpen(isError);
  }, [isError]);

  return (
    <>
      <Header />
      <Container className={classes.container} maxWidth="sm">
        <Typography component="h1" variant="h4">
          Вход
        </Typography>
        <Box component="form" className={classes.form}>
          <TextField
            InputProps={{ sx: { borderRadius: 30 } }}
            size="small"
            name="email"
            label="Почта"
            type="email"
            value={user?.email}
            onChange={(event) =>
              handleChange(event.target.name, event.target.value)
            }
            className={classes.fields}
          />
          <TextField
            InputProps={{ sx: { borderRadius: 30 } }}
            size="small"
            name="password"
            label="Пароль"
            type="password"
            value={user?.password}
            onChange={(event) =>
              handleChange(event.target.name, event.target.value)
            }
            className={classes.fields}
          />
          <Button
            className={classes.button}
            onClick={handleClick}
            variant="contained"
          >
            Войти
          </Button>
          <Link to={"/register"}>
            <Typography className={classes.signIn} component="h1" variant="h6">
              зарегистрироваться
            </Typography>
          </Link>
        </Box>
        <Snackbar
          anchorOrigin={{ vertical: 'bottom', horizontal: 'right'}}
          open={open}
          autoHideDuration={3000}
          onClose={handleClose}
          message={errorText}
        >
          <Alert
            onClose={handleClose}
            severity="error"
            variant="filled"
            sx={{ width: "100%" }}
          >
            {errorText}
          </Alert>
        </Snackbar>
      </Container>
    </>
  );
};

export default Login;
