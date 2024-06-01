import { FC, useEffect, useRef, useState } from "react";
import { Button, Container, Typography } from "@mui/material";
import AccordionElementKit from "./AccordionElement/AccordionElementKit";
import { useGetBoxesTypesQuery } from "../../../services/boxesService";
import { useAppDispatch } from "../../../store/redux";
import { useNavigate } from "react-router-dom";
import { setKitID, setKitTitle } from "../../../store/keyboardSlice";

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

  const [title, setTitle] = useState<string | undefined>(undefined)
  const [id, setId] = useState<string | undefined>(undefined)
  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const onClick = (value: TKits) => {
    if (!value.id) return;
    handleChoose(value.id);
    setId(value.id);
    setTitle(value.title)
  };

  const kitAdd = () => {
    dispatch(setKitTitle(title));
    dispatch(setKitID(id));
    navigate("/constructors")
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
            border: "1px solid #c1c0c0",
          }}
          variant="contained"
          onClick={() => kitAdd()}
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
export default KitsList;