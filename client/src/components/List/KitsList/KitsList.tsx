import { FC, useEffect, useRef, useState } from "react";
import List from "@mui/material/List";
import ListItem from "@mui/material/ListItem";
import ListItemButton from "@mui/material/ListItemButton";
import ListItemText from "@mui/material/ListItemText";
import { Button, Container, Typography } from "@mui/material";

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
};

const KitsList: FC<props> = ({ kits, handleChoose }) => {

  const onClick = (value: TKits) => {
    console.log(value);
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
        <Typography
          sx={{
            color: "white",
          }}> 
          Новый набор 
        </Typography>
        {kits?.map((value) => {
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
export default KitsList;
