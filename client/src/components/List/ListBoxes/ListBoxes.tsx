import { FC, useEffect, useRef, useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import IconButton from "@mui/material/IconButton";
import ExpandMoreIcon from "@mui/icons-material/ExpandMore";
import classes from "./ListBoxes.module.scss";
import {
  Accordion,
  AccordionActions,
  AccordionDetails,
  AccordionSummary,
  Button,
  Collapse,
  Container,
  Grid,
  ListItemIcon,
  TextField,
  Typography,
} from "@mui/material";
import AccordionElement from "./AccordionElement/AccordionElement";
import { HexColorPicker } from "react-colorful";
import { PopoverPicker } from "../../ColorPicker/PopoverPicker";

type TBoxes = {
  /**
   * id бокса
   */
  id?: string;
  /**
   * название бокса
   */
  title?: string;

  /**
   * id типа бокса
   */
  typeID?: string;

  /**
   * название типа бокса
   */
  typeTitle?: string;

  /**
   * дата создания бокса
   */
  creationDate?: string;
};

type props = {
  boxes40?: TBoxes[];
  boxes60?: TBoxes[];
  boxes75?: TBoxes[];
  boxes100?: TBoxes[];
  handleChoose: (id: string) => void;
  handleNew: (typeID: string) => void;
};

const BoxesList: FC<props> = ({
  boxes40,
  boxes60,
  boxes75,
  boxes100,
  handleChoose,
  handleNew,
}) => {

  const onClick = (value: TBoxes) => {
    if (!value.id) return;
    handleChoose(value.id);
  };

  const onClickNew = (typeID: string) => {
    if (!typeID) return;
    handleNew(typeID);
  };

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
        {
          <AccordionElement
            type="boxes100"
            name="100 percent"
            boxes={boxes100}
            handleChoose={onClick}
            handleNew={onClickNew}
          />
        }
        {
          <AccordionElement
            type="boxes75"
            name="75 percent"
            boxes={boxes75}
            handleChoose={onClick}
            handleNew={onClickNew}
          />
        }
        {
          <AccordionElement
            type="boxes60"
            name="60 percent"
            boxes={boxes60}
            handleChoose={onClick}
            handleNew={onClickNew}
          />
        }
        {
          <AccordionElement
            type="boxes40"
            name="40 percent"
            boxes={boxes40}
            handleChoose={onClick}
            handleNew={onClickNew}
          />
        }
      </Container>
      <Container>
        <Button
          sx={{
            m: "15px",
            width: "90%",
            borderRadius: "30px",
          }}
          variant="contained"
        >
          добавить
        </Button>
        <Button
          sx={{
            m: "15px",
            width: "90%",
            borderRadius: "30px",
          }}
          variant="contained"
        >
          назад
        </Button>
      </Container>
    </Container>
  );
};
export default BoxesList;
