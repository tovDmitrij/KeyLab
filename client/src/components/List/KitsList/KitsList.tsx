import { FC, useEffect, useRef, useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import { Button, Container, Typography } from "@mui/material";
import AccordionElementKit from "./AccordionElement/AccordionElementKit";
import { useGetBoxesTypesQuery } from "../../../services/boxesService";

type TKits = {
  /**
   * id кита
   */
  id?: string;
  /**
   * название кита
   */
  title?: string;
};

type props = {
  kits?: TKits[];
  handleChoose: (data: string) => void;
  handleNew: (data: string) => void;
};

const KitsList: FC<props> = ({  handleChoose, handleNew }) => {

  const { data : dataType } = useGetBoxesTypesQuery();

  const onClick = (value: TKits) => {
    if (!value.id) return;
    handleChoose(value.id);
  };

  const onClickNew = (boxTypeId: string) => {
    if (!boxTypeId) return;
    handleNew(boxTypeId);
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
        {dataType && dataType?.map((item : any) => {
          return (
            <AccordionElementKit
              name= {`${item.title} percent`}
              handleChoose={onClick}
              handleNew={onClickNew}
              boxTypeId={item.id}
            />
          )
        })}
       
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
export default KitsList;
