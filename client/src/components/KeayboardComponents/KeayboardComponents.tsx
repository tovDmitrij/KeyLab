import { FC, useEffect, useRef, useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import {
  Button,
  Container,
  ListItemButton,
  ListItemIcon,
  ListItemText,
  TextField,
  Tooltip,
  Typography,
} from "@mui/material";
import Preview from "../List/SwitchesList/Preview";
import { useAppSelector } from "../../store/redux";
import { useNavigate } from "react-router-dom";

type props = {
  kitID?: string;
  boxID?: string;
  switchTypeID?: string;
  onSave?: () => void;
};

const KeayboardComponents: FC<props> = ({ kitID, boxID, switchTypeID, onSave }) => {
  const { kitTitle, boxTitle, switchTitle} = useAppSelector(
    (state) => state.keyboardReduer
  );

  const navigate = useNavigate();
  const isTokenAvailable = !!localStorage.getItem("token");

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
            sx={{ textAlign: "center", minWidth: "100" }}
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
              dense
            >
              <ListItemIcon>
                <Preview
                  id={switchTypeID}
                  type="switch"
                  width={65}
                  height={65}
                />
              </ListItemIcon>
              <ListItemText
                sx={{
                  color: "#c1c0c0",
                  m: "5px",
                }}
                primary={`${switchTitle}`}
              />
            </ListItemButton>
          </ListItem>
          <ListItem
            sx={{ textAlign: "center", minWidth: "100" }}
            key={boxID}
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
              dense
            >
              <ListItemIcon>
                <Preview id={boxID} type="box" width={65} height={65} />
              </ListItemIcon>
              <ListItemText
                sx={{
                  color: "#c1c0c0",
                  m: "5px",
                }}
                primary={`${boxTitle}`}
              />
            </ListItemButton>
          </ListItem>
          <ListItem
            sx={{ textAlign: "center", minWidth: "100" }}
            key={kitID}
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
              dense
            >
              <ListItemIcon>
                <Preview id={kitID} type="kit" width={65} height={65} />
              </ListItemIcon>
              <ListItemText
                sx={{
                  color: "#c1c0c0",
                  m: "5px",
                }}
                primary={`${kitTitle}`}
              />
            </ListItemButton>
          </ListItem>
        </List>
      </Container>
      <Container
        disableGutters
        sx={{
          marginTop: "auto",
        }}
      >
        <Tooltip  title={!isTokenAvailable ? "Авторизируйтесь в системе" : ""}
      placement="top"
      disableHoverListener={isTokenAvailable}>
          <span>
            <Button
              sx={{
                m: "15px",
                width: "90%",
                borderRadius: "30px",
                border: "1px solid #c1c0c0",
              }}
              disabled={!isTokenAvailable}
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
          </span>
        </Tooltip>
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

export default KeayboardComponents;
