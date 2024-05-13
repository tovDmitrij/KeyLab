import { FC, useState } from "react";

import { Button, Container, Modal, TextField, Typography } from "@mui/material";
import { useAppSelector } from "../../store/redux";

type props = {
  open: boolean;
  handleCloseModal: () => void;
  onSubmitTitle: (title: string) => void;
};

const style = {
  position: "absolute" as "absolute",
  top: "50%",
  left: "50%",
  transform: "translate(-50%, -50%)",
  width: 400,
  bgcolor: "background.paper",
  border: "2px solid #000",
  boxShadow: 24,
  textAlign: "center",
  alignItems: "center",
  borderRadius: "30px",
  gap: "8px",
  p: 1,
};

const MoldaSetNameKeyboard: FC<props> = ({
  open,
  handleCloseModal,
  onSubmitTitle,
}) => {

  const { title } = useAppSelector(
    (state) => state.keyboardReduer
  );
  const [titleKeyboard, setTitleKeyboard] = useState<string>(title);
  
  return (
    <>
      <Modal open={open} onClose={handleCloseModal}>
        <Container sx={style}>
          <Typography variant="h6" component="h6" sx={{fontSize: 16, mt: "15px"}}>
            Придумайте название вашей клавиатуры
          </Typography>
          <TextField
            InputProps={{ sx: { borderRadius: 30, width: "320px"} }}
            sx={{ m: "20px" }}
            label="Название клавиатуры"
            size="small"
            name="title"
            variant="outlined"
            value={titleKeyboard}
            onChange={(event) => setTitleKeyboard(event.target.value)}
          />
          <Button
            onClick={() => titleKeyboard && onSubmitTitle(titleKeyboard)}
            variant="contained"
            sx={{ borderRadius: "30px", m: "10px" }}
          >
            Сохранить
          </Button>
        </Container>
      </Modal>
    </>
  );
};

export default MoldaSetNameKeyboard;
