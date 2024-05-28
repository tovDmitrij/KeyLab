import { FC, useEffect, useRef, useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import { Button, Container, TextField, Typography } from "@mui/material";
import { PopoverPicker } from "../../ColorPicker/PopoverPicker";
import { RgbaColor } from "react-colorful";

type props = {
  handleChooseColor: (data: RgbaColor) => void;
  saveKit: (data: any) => void;
  paintFull: () => void;
  title?: string;
};

const KeycapSettings: FC<props> = ({ handleChooseColor,  saveKit, title, paintFull }) => {
  const [color, setColor] = useState({ r: 205, g: 214, b: 199 });
  const [kitsTitle, setKitsTitle] = useState<string | undefined>(title);

  const onChange = (newColor: RgbaColor) => {
    handleChooseColor(newColor);
    setColor(newColor);
  };

  const save = () => {
    if (!kitsTitle) return;
    saveKit(kitsTitle);
  }

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
              value={kitsTitle}
              onChange={(event) => setKitsTitle(event.target.value)}
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
              Цвет клавиши:
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
          onClick={() => paintFull()}
        >
          <Typography
            sx={{
              color: "#c1c0c0",
            }}
          >
            ПОКРАСИТЬ ВСЕ
          </Typography>
        </Button>
        <Button
          sx={{
            m: "15px",
            width: "90%",
            borderRadius: "30px",
            border: "1px solid #c1c0c0",
          }}
          variant="contained"
          onClick={() => save()}
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
