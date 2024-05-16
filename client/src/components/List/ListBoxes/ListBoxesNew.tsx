import { FC, MutableRefObject, useEffect, useState } from "react";

import {
  Button,
  Container,
  List,
  ListItem,
  TextField,
  Typography,
} from "@mui/material";
import { PopoverPicker } from "../../ColorPicker/PopoverPicker";
import { RgbaColor } from "react-colorful";

type props = {
  handleChooseColor: (data: any) => void;
  saveNewBox: (title: string) => void;
};

const ListBoxesNew: FC<props> = ({ handleChooseColor, saveNewBox }) => {
  const [color, setColor] = useState({ r: 205, g: 214, b: 199 });
  const [title, setTitle] = useState<string>();

  const onChange = (newColor: RgbaColor) => {
    handleChooseColor(newColor);
    setColor(newColor);
  };

  const onSave = () => {
    if (!title) return;
    saveNewBox(title);
  };

  return (
    <Container
      disableGutters
      sx={{
        display: "flex",
        flexDirection: "column",
        textAlign: "center",
        bgcolor: "#2A2A2A",
        height: "100vh",
        overflow: "hidden",
      }}
    >
      <Container
        disableGutters
        sx={{
          mt: "66px",
          height: "76%",
          width: "100%",
          position: "relative",
          overflowY: "auto",
        }}
      >
        <List>
          <ListItem
            sx={{
              height: "80px",
              borderTop: "1px solid grey",
              borderBottom: "1px solid grey",
              margin: "0 0 -1px 0px",
            }}
            disablePadding
          >
            <Typography color={"#c1c0c0"} sx={{ m: "10px" }}>
              Имя
            </Typography>
            <TextField
              InputProps={{
                sx: {
                  color: "#c1c0c0",
                  backgroundColor: "#191919",
                },
              }}
              size="small"
              name="New"
              type="text"
              sx={{
                m: "10px",
                width: "100%",
              }}
              onChange={(event) => setTitle(event.target.value)}
            />
          </ListItem>
          <ListItem
            sx={{
              height: "80px",
              borderTop: "1px solid grey",
              borderBottom: "1px solid grey",
              margin: "0 0 -1px 0px",
            }}
            disablePadding
          >
            <Typography color={"#c1c0c0"} sx={{ m: "10px" }}>
              Цвет
            </Typography>
            <PopoverPicker color={color} onChange={onChange} />
          </ListItem>
        </List>
      </Container>
      <Container
        disableGutters
        sx={{
          marginTop: "auto",
        }}
      >
        <Button
          sx={{
            m: "15px",
            width: "90%",
            borderRadius: "30px",
            border: "1px solid #c1c0c0",
          }}
          variant="contained"
          onClick={onSave}
        >
          <Typography
            sx={{
              color: "#c1c0c0",
            }}
          >
            добавить
          </Typography>
        </Button>
      </Container>
    </Container>
  );
};

export default ListBoxesNew;
