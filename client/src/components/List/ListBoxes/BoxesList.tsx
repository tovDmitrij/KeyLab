import { FC, useEffect, useRef, useState } from "react";
import {
  Button,
  Container,
  Typography,
} from "@mui/material";
import AccordionElement from "./AccordionElement/AccordionElement";
import { useNavigate } from "react-router-dom";
import { useGetBoxesTypesQuery } from "../../../services/boxesService";
import { useAppDispatch, useAppSelector } from "../../../store/redux";
import { setBoxID, setBoxTitle, setBoxTypeId } from "../../../store/keyboardSlice";

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
  handleChoose: (id: string) => void;
  handleNew: (idType: string, idBaseBox : string) => void;
};

const BoxesList: FC<props> = ({
  handleChoose,
  handleNew,
}) => {
  const { typeSizeKit, boxID } = useAppSelector(
    (state) => state.keyboardReduer
  );

  const { data : dataType } = useGetBoxesTypesQuery();

  const [title, setTitle] = useState<string | undefined>(undefined)
  const [id, setId] = useState<string | undefined>(undefined)
  const [typeID,  setTypeID] = useState<string | undefined>(undefined)

  const navigate = useNavigate();
  const dispatch = useAppDispatch();

  const boxAdd = () => {
    if (!id) {
      navigate("/constructors")
      return;
    }
    dispatch(setBoxTitle(title));
    dispatch(setBoxID(id));
    dispatch(setBoxTypeId(typeID));
    navigate("/constructors")
  }


  const onClick = (value: TBoxes) => {
    if (!value.id) return;
    handleChoose(value.id);
    setTitle(value.title);
    setId(value.id);
    setTypeID(value.typeID);
  };

  const onClickNew = (idType: string, idBaseBox : string) => {
    if (!idType) return;
    handleNew(idType, idBaseBox);
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
          pr: "0",
          mt: "66px",
          height: "76%",
          width: "100%",
          position: "relative",
          overflowY: "auto",
        }}
      >
        {dataType && dataType?.map((item : any) => {
          if (item.id === typeSizeKit || typeSizeKit === undefined)
          return (
            <AccordionElement
              name= {`${item.title} percent`}
              handleChoose={onClick}
              handleNew={onClickNew}
              boxTypeId={item.id}
            />
          )
        })}
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
          onClick={() => boxAdd()}
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
export default BoxesList;
