import { FC, useEffect, useRef, useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import IconButton from "@mui/material/IconButton";

import classes from "./SwitchesList.module.scss";
import { Button, Container } from "@mui/material";

type TSwitches = {
  /**
   * id свитча
   */
  id?: string;
  /**
   * название свитча
   */
  title?: string;
};

type props = {
  switches: TSwitches[];
  handleChoose: (data: string) => void;
};

const SwitchesList: FC<props> = ({ switches, handleChoose }) => {
  const onClick = (value: TSwitches) => {
    if (!value.id) return;
    handleChoose(value.id);
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
      <List
        sx={{
          mt: "66px",
          height: "76%",
          position: "relative",
          overflowY: "auto",
        }}
      >
        {switches?.map((value) => {
          const labelId = `checkbox-list-label-${value}`;

          return (
            <ListItem
              sx={{ textAlign: "center", minWidth: "100" }}
              key={value.id}
              disablePadding
            >
              <ListItemButton
                role={undefined}
                onClick={() => onClick(value)}
                dense
              >
                <ListItemText
                  sx={{
                    color: "white",
                    m: "5px",
                  }}
                  id={labelId}
                  primary={`${value.title}`}
                />
              </ListItemButton>
            </ListItem>
          );
        })}
      </List>
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
export default SwitchesList;
