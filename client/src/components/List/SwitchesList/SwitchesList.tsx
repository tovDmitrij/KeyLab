import { FC, useEffect, useRef, useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText"
import { Button, Container, ListItemIcon, Typography } from "@mui/material";
import Preview from "./Preview";
import { useNavigate } from "react-router-dom";
import { useAppDispatch } from "../../../store/redux";
import { setSwitchTitle, setSwitchTypeID } from "../../../store/keyboardSlice";

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
  const [title, setTitle] = useState<string | undefined>(undefined)
  const [id, setId] = useState<string | undefined>(undefined)

  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const switchAdd = () => {
    dispatch(setSwitchTitle(title));
    dispatch(setSwitchTypeID(id));
    navigate("/constructors")
  }

  const onClick = (value: TSwitches) => {
    if (!value.id) return;
    setTitle(value.title);
    setId(value.id);
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
          position: "relative",
          overflowY: "auto",
        }}
      >
        {switches?.map((value) => {
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
                    <Preview
                      id = {value?.id}
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
          disabled={!id}
          onClick={() => switchAdd()}
          variant="contained"
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
