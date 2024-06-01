import { FC, useEffect, useState } from "react";

import ArrowDropDownIcon from "@mui/icons-material/ArrowDropDown";
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
import {
  useLazyGetAuthKitsQuery,
  useLazyGetDefaultKitsQuery,
} from "../../../../services/kitsService";
import Preview from "../../SwitchesList/Preview";

type TKits = {
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
  name: string;
  handleChoose: (data: TKits, boxTypeId?: string) => void;
  handleNew: (data: string) => void;
  boxTypeId: string;
};

const AccordionElementKit: FC<props> = ({
  boxTypeId,
  name,
  handleChoose,
  handleNew,
}) => {
  const [uniqueKits, setUniqueKits] = useState<TKits[]>();
  const [getBaseKits, { data: kitsBase }] = useLazyGetDefaultKitsQuery();
  const [getAuthKits, { data: kits }] = useLazyGetAuthKitsQuery();

  const onClick = (value: TKits) => {
    if (!value.id) return;
    handleChoose(value, boxTypeId);
  };

  const onClickNew = () => {
    handleNew(boxTypeId);
  };

  useEffect(() => {
    const data = {
      page: 1,
      pageSize: 100,
      typeID: boxTypeId,
    };
    getAuthKits(data);
    getBaseKits(data);
  }, []);

  useEffect(() => {
    if (kitsBase && kits)
      setUniqueKits(Array.from(new Set(kitsBase.concat(kits))));
    if (kitsBase) setUniqueKits(kitsBase);
    if (kits) setUniqueKits(kits);
  }, [kitsBase, kits]);

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
            {uniqueKits?.map((value) => {
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
                            id={value?.id}
                            type="kit"
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
                    color: "white",
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

export default AccordionElementKit;
