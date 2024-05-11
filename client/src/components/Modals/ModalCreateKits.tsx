import { FC, useState } from "react";

import { Button, Container, Modal, TextField, Typography } from "@mui/material";

type props = {
  open: boolean;
  handleCloseModal: () => void;
  onSubmitTitleModal: (title: string) => void;
}

const style = {
  position: 'absolute' as 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 400,
  bgcolor: 'background.paper',
  border: '2px solid #000',
  boxShadow: 24,
  textAlign: 'center',
  alignItems: 'center',
  borderRadius: "30px",
  gap: "8px",
  p: 1,
};

const ModalCreateKits:FC<props> = ({open, handleCloseModal, onSubmitTitleModal}) => {
  const [title, setTitle] = useState<string>();
  return (
  <>
     <Modal
      open={open}
      onClose={handleCloseModal}
    >
    <Container   sx={style}>
      <Typography
        variant="h6"
        component="h3"
      >
        Введите название набора клавиш
      </Typography>
      <TextField
        InputProps={{ sx: { borderRadius: 30, width: "320px" } }}
        sx={{ m: "10px" }}
        label="Название набора"
        size="small"
        name="title"
        variant="outlined"
        value={title}
        onChange={(event) => setTitle(event.target.value)}
      />
      <Button
        onClick={() => title && onSubmitTitleModal(title)}
        variant="contained"
        sx={{borderRadius: "30px", m: "10px"}}
      >
        Продолжить 
      </Button>
    </Container>
  </Modal></> 
  )
}

export default ModalCreateKits;