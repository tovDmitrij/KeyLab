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
import { usePostBoxMutation } from "../../../services/boxesService";
import { GLTFExporter } from "three/examples/jsm/Addons.js";

type props = {
  handleChooseColor: (data: any) => void;
  saveNewBox: (title: string) => void;
};

const ListBoxesNew: FC<props> = ({
  handleChooseColor,
  saveNewBox,
}) => {

  const [color, setColor] = useState({ r: 200, g: 150, b: 35, a: 0.5 });
  const [title, setTitle] = useState<string>();

  const onChange = (newColor: RgbaColor) => {
    handleChooseColor(newColor);
    setColor(newColor);
  }

  const onSave = () => {
    if (!title) return;
    saveNewBox(title);
  }

  return (
    <Container
      disableGutters
      sx={{
        width: "100%",
        textAlign: "center",
        bgcolor: "#2A2A2A",
        height: "100vh",
        overflow: "hidden",
      }}
    >
      <Container
        disableGutters
        sx={{
          pr: "0",
          mt: "66px",
          height: "76%",
          width: "100%",
          position: "relative",
          overflowY: "auto",
        }}
      >
        <List>
          <ListItem disablePadding>
            <Typography color={"white"} sx={{ m: "10px" }}>
              Name
            </Typography>
            <TextField
              InputProps={{
                sx: {
                  border: "1px solid white",
                  color: "white",
                },
              }}
              size="small"
              name="New"
              type="text"
              sx={{
                m: "10px",
                width: "100%",
              }}
              onChange={(event) =>
                setTitle(event.target.value)
              }
            />
          </ListItem>
          <ListItem disablePadding>
            <Typography color={"white"} sx={{ m: "10px" }}>
              Color
            </Typography>
            <PopoverPicker color={color} onChange={onChange} />
          </ListItem>
        </List>
      </Container>
      <Container>
        <Button
          sx={{
            m: "15px",
            width: "90%",
            borderRadius: "30px",
          }}
          variant="contained"
          onClick={onSave}
        >
          Сохранить 
        </Button>
      </Container>
    </Container>
  );
};

export default ListBoxesNew;
