import { FC, useEffect, useState } from "react";

import {
  Accordion,
  AccordionDetails,
  AccordionSummary,
  Grid,
  List,
  ListItem,
  ListItemButton,
  ListItemText,
  Tooltip,
  Typography,
} from "@mui/material";
import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
import {
  useGetAuthBoxesQuery,
  useGetBoxesQuery,
  useGetDefaultBoxesQuery,
  useLazyGetAuthBoxesQuery,
  useLazyGetDefaultBoxesQuery,
} from "../../../../services/boxesService";
import Preview from "../../SwitchesList/Preview";

type TBoxes = {
  /**
   * id бокса
   */
  id?: string;
  /**
   * название бокса
   */
  title?: string;
};

type props = {
  boxTypeId: string;
  name: string;
  handleChoose: (data: TBoxes) => void;
  handleNew: (idType: string, idBaseBox: string) => void;
};

const AccordionElement: FC<props> = ({
  boxTypeId,
  name,
  handleChoose,
  handleNew,
}) => {
  const [getAuthBoxes, { data: boxes }] = useLazyGetAuthBoxesQuery();

  const [getBaseBoxes, { data: boxesBase }] = useLazyGetDefaultBoxesQuery();

  const [uniqueBoxes, setUniqueBoxes] = useState<TBoxes[]>();

  const onClick = (value: TBoxes) => {
    if (!value.id) return;
    handleChoose(value);
  };

  const onClickNew = () => {
    if (!boxesBase || !boxesBase[0]?.id) return;
    handleNew(boxTypeId, boxesBase[0]?.id as string);
  };

  useEffect(() => {
    const data = {
      page: 1,
      pageSize: 100,
      typeID: boxTypeId,
    };
    getAuthBoxes(data);
    getBaseBoxes(data);
  }, []);

  useEffect(() => {
    if (boxesBase && boxes)
      setUniqueBoxes(Array.from(new Set(boxesBase.concat(boxes))));
    if (boxesBase) setUniqueBoxes(boxesBase);
    if (boxes) setUniqueBoxes(boxes);
  }, [boxesBase, boxes]);

  return (
    <Accordion disableGutters={true}>
      <AccordionSummary
        sx={{
          bgcolor: "#2A2A2A",
          color: "#c1c0c0",
          borderTop: "1px solid grey",
          borderBottom: "1px solid grey",
          margin: "0 0 -1px 0px",
        }}
        expandIcon={
          <ArrowDropDownIcon sx={{ color: "#AEAEAE", fontSize: 40 }} />
        }
      >
        <Typography
          sx={{
            textAlign: "center",
            margin: "15px",
          }}
        >
          {name}
        </Typography>
      </AccordionSummary>
      <AccordionDetails
        sx={{
          p: 0,
          bgcolor: "#2A2A2A",
          color: "#FFFFFF",
        }}
      >
        <Grid container direction="column">
          <List disablePadding>
            {uniqueBoxes?.map((value) => {
              const labelId = `checkbox-list-label-${value}`;

              return (
                <>
                  <ListItem
                    sx={{ minWidth: "100" }}
                    key={value.id}
                    disablePadding
                  >
                    <Tooltip
                      placement="left"
                      title={
                        <>
                          <Preview
                            id = {value?.id}
                            type="box"
                            
                            width={300}
                            height={165}
                          />
                        </>
                      }
                      arrow
                    >
                      <ListItemButton
                        sx={{
                          textAlign: "center",
                          margin: "0 0 -1px 0px",
                          border: "1px solid grey",
                          "&:hover": {
                            backgroundColor: "grey",
                          },
                        }}
                        role={undefined}
                        onClick={() => onClick(value)}
                        dense
                      >
                        <ListItemText
                          sx={{
                            textAlign: "center",
                            color: "#c1c0c0",
                            m: "5px",
                          }}
                          id={labelId}
                          primary={`${value.title}`}
                        />
                      </ListItemButton>
                    </Tooltip>
                  </ListItem>
                </>
              );
            })}
            <ListItem sx={{ minWidth: "100" }} disablePadding>
              {localStorage.getItem("token") && <ListItemButton
                sx={{
                  textAlign: "center",
                  border: "1px solid grey",
                  margin: "8px 0 -1px 0px",
                  "&:hover": {
                    backgroundColor: "grey",
                  },
                }}
                role={undefined}
                onClick={() => onClickNew()}
                dense
              >
                <ListItemText
                  sx={{
                    textAlign: "center",
                    color: "#c1c0c0",
                    m: "5px",
                  }}
                  primary={`Создать`}
                />
              </ListItemButton>}
            </ListItem>
          </List>
        </Grid>
      </AccordionDetails>
    </Accordion>
  );
};

export default AccordionElement;
