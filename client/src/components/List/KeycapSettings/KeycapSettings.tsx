import { FC, useEffect, useRef, useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import { Button, Container, Typography } from "@mui/material";
import { PopoverPicker } from "../../ColorPicker/PopoverPicker";
import { RgbaColor } from "react-colorful";

type TKeycaps = {
  /**
   * id кита
   */
  id?: string;
  /**
   * название кита
   */
  title?: string;

  /**
   * дата создания бокса
   */
  creationDate?: string;
};

type props = {

  handleChooseColor: (data: RgbaColor) => void;
  saveNewKeycap: (data: any) => void;
};

const KeycapSettings: FC<props> = ({ handleChooseColor, saveNewKeycap }) => {
  const [color, setColor] = useState({ r: 205, g: 214, b: 199 });

  const onChange = (newColor: RgbaColor) => {
    handleChooseColor(newColor);
    setColor(newColor);
  };

  const onSave = () => {

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
              Название набора:
            </Typography>
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
              Выбрана клавиша: 
            </Typography>
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
              Цвет клавиши:
            </Typography>
            <PopoverPicker color={color} onChange={onChange} />
          </ListItem>
        </List>
      </Container>
      <Container
        disableGutters
        sx={{
          //height: "76%",
          //position: "rela",
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
            сохранить
          </Typography>
        </Button>
      </Container>
    </Container>
  );
};

export default KeycapSettings;