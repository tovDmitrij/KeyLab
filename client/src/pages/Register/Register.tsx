import React, { useEffect, useState } from "react";
import { Link, useNavigate } from "react-router-dom";

import {
  Alert,
  Box,
  Button,
  Container,
  Modal,
  Snackbar,
  TextField,
  Typography,
} from "@mui/material";

import {
  useSignUpMutation,
  useVerifEmailMutation,
} from "../../services/userService";
import Header from "../../components/Header/Header";

import classes from "./Register.module.scss";

/**
 * Тип данных представляет информацию о пользователе для регистрации.
 */
type TUser = {
  /**
   * Псевдоним пользователя.
   */
  nickname: string | null;
  /**
   * Пароль пользователя.
   */
  password: string | null;
  /**
   * Павторенный пароль пользователя.
   */
  repeatedPassword: string | null;
  /**
   * Email
   */
  email: string | null;
  /**
   * Пароль пользователя.
   */
  emailCode: string | null;
};

const Register = () => {
  const navigate = useNavigate();
  const [open, setOpen] = useState<boolean>(false);

  const [openAlert, setOpenAlert] = useState<boolean>(false);

  const [errorText, setErrorText] = useState<string>();
  const [isError, setIsError] = useState<boolean>(false);

  const [newUser, setNewUser] = useState<TUser>({
    nickname: null,
    password: null,
    repeatedPassword: null,
    email: null,
    emailCode: null,
  });
  const [registerUser] = useSignUpMutation();
  const [sendCode] = useVerifEmailMutation();

  const isValidEmail = (email: string) => {
    const emailRegex = /^[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,}$/i;

    return emailRegex.test(email as string);
  };

  const handleChange = (argName: string, argValue: string) => {
    setNewUser({
      ...newUser,
      [argName]: argValue,
    });
  };

  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const onSubmitEmail = () => {
    if (!newUser.email) return;
    if (!isValidEmail(newUser.email)) {
      setIsError(true);
      setErrorText("Почта не валидная. Пример: ivanov@mail.ru");
      return;
    }

    handleOpen();
    newUser.emailCode = null;
    sendCode({ email: newUser.email })
      .unwrap()
      .catch((error) => {
        setIsError(true);
        setErrorText(error.data);
      });
  };

  const onSubmit = () => {
    handleClose();
    if (newUser) {
      registerUser(newUser)
        .unwrap()
        .then(() => {
          navigate("/login");
        })
        .catch((error) => {
          setIsError(true);
          setErrorText(error.data);
        });
    }
  };

  const handleCloseAlert = (
    event: React.SyntheticEvent | Event,
    reason?: string
  ) => {
    if (reason === "clickaway") {
      return;
    }

    setIsError(false);
  };

  useEffect(() => {
    setOpenAlert(isError);
  }, [isError]);

  return (
    <>
      <Header />
      <Container className={classes.container} maxWidth="sm">
        <Typography component="h1" variant="h4">
          Регистрация
        </Typography>
        <Box component="form" className={classes.form}>
          <TextField
            InputProps={{ sx: { borderRadius: 30 } }}
            size="small"
            name="nickname"
            label="Nickname"
            variant="outlined"
            type="text"
            value={newUser?.nickname}
            onChange={(event) =>
              handleChange(event.target.name, event.target.value)
            }
            className={classes.fields}
          />
          <TextField
            InputProps={{ sx: { borderRadius: 30 } }}
            size="small"
            name="email"
            label="Почта"
            variant="outlined"
            type="email"
            value={newUser?.email}
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
            variant="outlined"
            type="password"
            value={newUser?.password}
            onChange={(event) =>
              handleChange(event.target.name, event.target.value)
            }
            className={classes.fields}
          />
          <TextField
            InputProps={{ sx: { borderRadius: 30 } }}
            size="small"
            name="repeatedPassword"
            label="Павторите пароль"
            variant="outlined"
            type="password"
            value={newUser?.repeatedPassword}
            onChange={(event) =>
              handleChange(event.target.name, event.target.value)
            }
            className={classes.fields}
          />
          <Button
            className={classes.button}
            onClick={onSubmitEmail}
            variant="contained"
          >
            Зарегистрироваться
          </Button>
          <Link to={"/login"}>
            <Typography className={classes.signUp} component="h1" variant="h6">
              войти
            </Typography>
          </Link>
        </Box>
      </Container>
      <Snackbar
        anchorOrigin={{ vertical: "bottom", horizontal: "right" }}
        open={openAlert}
        autoHideDuration={3000}
        onClose={handleCloseAlert}
        message={errorText}
      >
        <Alert
          onClose={handleCloseAlert}
          severity="error"
          variant="filled"
          sx={{ width: "100%" }}
        >
          {errorText}
        </Alert>
      </Snackbar>
      {/* TODO: Перенести модлку в отдельный компонент */}
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Container   sx={{width: "500px"}} className={classes.modal}>
          <Typography
            className={classes.textUp}
            id="modal-modal-title"
            variant="h6"
            component="h3"
          >
            Введите код, который мы отправили
          </Typography>
          <Typography
            className={classes.textDown}
            id="modal-modal-title"
            variant="h6"
            component="h4"
          >
            вам на почту
          </Typography>
          <TextField
            InputProps={{ sx: { borderRadius: 30 } }}
            label="Код"
            size="small"
            name="emailCode"
            variant="outlined"
            value={newUser?.emailCode}
            onChange={(event) =>
              handleChange(event.target.name, event.target.value)
            }
            className={classes.modalFields}
          />
          <Button
            className={classes.buttonModal}
            onClick={onSubmit}
            variant="contained"
          >
            Отправить код
          </Button>
        </Container>
      </Modal>
    </>
  );
};

export default Register;
