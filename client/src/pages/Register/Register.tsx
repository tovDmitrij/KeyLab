import {
  Box,
  Button,
  Container,
  Modal,
  TextField,
  Typography,
} from "@mui/material";
import React, { useState } from "react";
import classes from "./Register.module.scss";
import { useSignUpMutation, useVerifEmailMutation } from "../../store";
import { useNavigate } from "react-router-dom";

/**
 * Тип данных представляет информацию о пользователе для авторизации.
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
  const [open, setOpen] = useState(false);
  const [newUser, setNewUser] = useState<TUser>({
    nickname: null,
    password: null,
    repeatedPassword: null,
    email: null,
    emailCode: null,
  });
  const [registerUser] = useSignUpMutation();
  const [sendCode] = useVerifEmailMutation();

  const handleChange = (argName: string, argValue: string) => {
    setNewUser({
      ...newUser,
      [argName]: argValue,
    });
  };

  const handleOpen = () => setOpen(true);
  const handleClose = () => setOpen(false);

  const onSubmitEmail = async () => {
    handleOpen(); 
    newUser.emailCode = null;
    if (newUser.email) {
      await sendCode({ email: newUser.email });
    }
  };
  const onSubmit = async () => {
    handleClose();
    if (newUser) {
      await registerUser(newUser);
      navigate("/login")
    }
    
  };

  return (
    <div>
      <Container className={classes.container} maxWidth="sm">
        <Typography component="h1" variant="h5">
          Странциа регистрации
        </Typography>
        <Box component="form" className={classes.form}>
          <TextField
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
        </Box>
      </Container>
      <Modal
        open={open}
        onClose={handleClose}
        aria-labelledby="modal-modal-title"
        aria-describedby="modal-modal-description"
      >
        <Box className={classes.modal}>
          <Typography id="modal-modal-title" variant="h6" component="h2">
            Подтвердите почту {newUser.email}
          </Typography>
          <TextField
            name="emailCode"
            variant="outlined"
            value={newUser?.emailCode}
            onChange={(event) =>
              handleChange(event.target.name, event.target.value)
            }
            className={classes.fields}
          />
          <Button
            className={classes.buttonModal}
            onClick={onSubmit}
            variant="contained"
          >
            Отправить код
          </Button>
        </Box>
      </Modal>
    </div>
  );
};

export default Register;
