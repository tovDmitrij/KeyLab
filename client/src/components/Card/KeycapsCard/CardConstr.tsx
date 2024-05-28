import { FC, useEffect, useState } from "react";

import Card from "@mui/material/Card";
import CardContent from "@mui/material/CardContent";
import Typography from "@mui/material/Typography";
import { CardActionArea } from "@mui/material";
import IconPlus from "../../Icons/IconPlus/IconPlus";

import classes from "./KeycapsCard.module.scss";
import { useNavigate } from "react-router-dom";
import { useAppSelector } from "../../../store/redux";
import Preview from "../../List/SwitchesList/Preview";

const CardConstr: FC<any> = ({type}) => {

  const [link, setLink] = useState<string>()
  const [title, setTilte] = useState<string>()
  const [id, setId] = useState<string>()

  const navigate = useNavigate();
  const { kitID, kitTitle, switchTitle, switchTypeID, boxID, boxTitle } = useAppSelector(
    (state) => state.keyboardReduer
  );

  useEffect(() => {
    if (type  === "box") {
      setLink("/constrBoxes")
      boxTitle ? setTilte(boxTitle) : setTilte('Выберите размер клавиатуры')
      setId(boxID);
    } else if (type  === "switch") {
      setLink("/constrSwitch")
      switchTitle ? setTilte(switchTitle) : setTilte('Выберите переключатель')
      setId(switchTypeID);
    } else if (type  === "kit") { 
      setLink("/constrKeys")
      kitTitle ? setTilte(kitTitle) : setTilte('Выберите набор клавиш')
      setId(kitID);
    }
  }, [kitID, switchTypeID, boxID])

  return (
    <Card className={classes.card} sx={{ borderRadius: 10 }}>
      <CardActionArea onClick={() => link &&  navigate(link)}>
        <CardContent className={classes.card_content}>
        {title?.includes('Выберите') ? (
            <>
              <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
                {title}
              </Typography>
              <IconPlus sx={{ fontSize: 50 }} />
            </>
          ) : (
            <>
              {id && <Preview id={id} type={type} width={type === 'switch' ? 165 : 300} height={165} />}
              <Typography sx={{ mt: 1, fontSize: 20, color: "#AEAEAE" }}>
                {title}
              </Typography>
            </>
          )}
        </CardContent>
      </CardActionArea>
    </Card>
  );
};

export default CardConstr;
