import { FC, useEffect, useRef, useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import IconButton from "@mui/material/IconButton";

import classes from "./SwitchesList.module.scss";
import { Button, Container, ListItemIcon, Typography } from "@mui/material";
import PlusImage from "./Plus.png";
import Preview from "./Preview";
import { useNavigate } from "react-router-dom";

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
  const navigate = useNavigate()
  const onClick = (value: TSwitches) => {
    if (!value.id) return;
    handleChoose(value.id);
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
      <List
        sx={{
          mt: "66px",
          //height: "76%",
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
                sx={{
                  textAlign: "center",
                  minWidth: "100",
                  borderTop: "1px solid grey",
                  borderBottom: "1px solid grey",
                  margin: "0 0 -1px 0px",
                }}
                role={undefined}
                onClick={() => onClick(value)}
                dense
              >
                <ListItemIcon>
                  <Preview type="switch" id={value.id} />
                </ListItemIcon>
                <ListItemText
                  sx={{
                    color: "#c1c0c0",
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
          //onClick={() => set }
        >
          <Typography
            sx={{
              color: "#c1c0c0",
            }}
          >
            добавить
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
          onClick={() => navigate("/constructors")}
        >
          <Typography
            sx={{
              color: "#c1c0c0",
            }}
          >
            назад
          </Typography>
        </Button>
      </Container>
    </Container>
  );
};
export default SwitchesList;
