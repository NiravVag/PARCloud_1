import React, { useEffect, useState, useMemo } from 'react';

import ReactTable from '../../../components/Common/ReactTable/ReactTable';

import TableFilter from "../../../components/Common/ReactTable/TableFilter";

import {
    Alert,
    Badge,
    Button,
    ButtonGroup,
    Card,
    Form,
    InputGroup,
    Navbar,
    Nav,
    Table,
    Container,
    Row,
    Col,
    OverlayTrigger,
    Tooltip,
    ListGroup,
    Modal,
    FormControl,
    Spinner
} from "react-bootstrap";

import MeasureScale from './../../Common/MeasureScale';

import moment from "moment";

const ScalesPanel = (props) => {   
    const [isLoading, setIsLoading] = useState(true);
    const [measuredScale, setMeasuredScale] = useState();
    const [scales, setScales] = useState([]);

    let scalesTable = undefined;
    let data = [];

    const url = "api/Scale/GetByTenantId/" + props.tenant.id;   

    useEffect(() => {
        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setScales([]);
                } else {
                    const data = await response.json();
                    setScales(data.scales);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };
        fetchData(url);
    }, [url, props.onRefresh]);

    //const handleRequestMeasure = async (scale) => {
    //  setMeasuredScale(scale);
    //    try {
    //        const response = await fetch("api/Scale/RequestBinMeasurement/" + scale.binId);

    //        if (response.status === 204) {               
    //        } else if (response.status === 500) {                
    //        }
    //        else {
    //            const data = await response.json();
    //        }            
    //    } catch (error) {            
    //        throw error;
    //    }
    //  setMeasuredScale(undefined);
    //}

    if (scales && scales.length > 0) {        
        data = scales.map((scale, key) => {
            let scaleMeasureButton;
            

            if (scale.binId) {
                let modalHeader = "Request Scale Weight Value",
                    modalContent = <MeasureScale scale={scale} onCancel={props.onCloseModal} />;
                
                scaleMeasureButton = <Button
                    onClick={(e) => {
                        props.onOpenModal(modalHeader, modalContent);
                    }}
                    title="Request scale measurement"
                    variant="primary"
                    size="sm"
                    className="btn-primary edit"
                >
                    Measure
                </Button>;
               

                if (measuredScale && measuredScale.binId == scale.binId) {
                    scaleMeasureButton = <Spinner animation="border" variant="primary" />;
                }
            } else {
                scaleMeasureButton = <Button
                    variant="default"
                    size="sm"
                    className="btn-danger edit"
                >
                   No Bin
                </Button>
            }

            let statusButton = (scale.isRunning) ?
                <Alert variant="success" className="status">Online</Alert>
                : <Alert variant="danger" className="status">Offline</Alert>;

            return {
                id: scale.id,
                location: scale.locationName,
                address: scale.address,
                itemName: scale.itemName,
                itemNumber: scale.itemNumber,
                scaleWeight: scale.scaleWeight,
                controllerIp: scale.controllerIp,
                lastCommunication: moment.utc(scale.lastCommunication).local().format("MM/DD/YYYY hh:mm:ss A"),
                status: scale.isRunning,
                statusContent: statusButton,
                actions: (<div className="actions-right">{scaleMeasureButton}</div>),
            };
        });
    }

    const statusSortFactory = () => {
        const STATUS = { true: 0, false: 1 };
        const sortFunc = (a, b) => {           
            const aStatus = a.values.status;
            const bStatus = b.values.status;
            const aVal = STATUS[aStatus];
            const bVal = STATUS[bStatus];
            return aVal < bVal ? -1 : 1;
        };
        return sortFunc;
    };

    const statusSort = useMemo(statusSortFactory);

    scalesTable = (
        <ReactTable
            data={data}
            columns={[
                {
                    Header: "Location",
                    accessor: "location",
                    className: 'rt-first',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Address",
                    accessor: "address",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Item Name",
                    accessor: "itemName",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                    canFilter: true
                },
                {
                    Header: "Item Number",
                    accessor: "itemNumber",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },                
                {
                    Header: "Scale Weight",
                    accessor: "scaleWeight",
                    className: 'rt-custom',
                   
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                },                
                {
                    Header: "Controller IP",
                    accessor: "controllerIp",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 140,
                },
                {
                    Header: "Last Communication (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "lastCommunication",
                    className: 'rt-custom',
                    maxWidth: 140,
                    minWidth: 140,
                    width: 160,
                },
                {
                    Header: "StatusHidden",
                    accessor: "status",
                    className: 'rt-custom',
                    show: false,
                },
                {
                    Header: "Status",
                    accessor: "statusContent",
                    className: 'rt-custom',
                    sortType: statusSort,
                    maxWidth: 100,
                    minWidth: 80,
                    width: 100,
                },
                {
                    Header: "Actions",
                    accessor: "actions",
                    className: 'rt-custom',
                    sortable: false,
                    filterable: false,
                    maxWidth: 70,
                    minWidth: 70,

                    width: 70,
                },
            ]}
            /*
                You can choose between primary-pagination, info-pagination, success-pagination, warning-pagination, danger-pagination or none - which will make the pagination buttons gray
            */
            header={TableFilter}
            className="-striped -highlight primary-pagination"
        />
    );

    return (
        <>            
            <Card.Body className="table-full-width">
                <Row>
                    <Col className="mx-auto setup-scales-table pt-2">
                        {scalesTable}
                    </Col>
                </Row>
            </Card.Body>           
        </>
    );
}

export default ScalesPanel;
