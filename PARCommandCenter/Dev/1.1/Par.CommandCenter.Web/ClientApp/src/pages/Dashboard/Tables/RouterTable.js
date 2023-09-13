import React, { useEffect, useState } from 'react';

import ReactTable from '../../../components/Common/ReactTable/ReactTable';

import {
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


import { CSVLink, CSVDownload } from 'react-csv';
import moment from "moment-timezone";


const RouterTable = (props) => {

    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState();
    const [requestedAction, setRequestedAction] = useState();

    const [restartError, setRestartError] = useState();


    let startDateValue = "", endDateValue = "";
    if (props.filter.startDate && props.filter.endDate) {
        startDateValue = props.filter.startDate.format("MM/DD/YYYY");
        endDateValue = props.filter.endDate.format("MM/DD/YYYY");
    }

    const url = props.tableMetadata.dataUrl;

    let table = undefined;
    let tableData = [];
    let tableHeader = [];    

    useEffect(() => {
        const fetchData = async (url, ids) => {
            try {
                setIsLoading(true);

                const response = await fetch(url, {
                    method: 'POST',
                    headers: {
                        'Content-Type': 'application/json',
                    },
                    body: JSON.stringify({ healthCheckIds: ids, filter: props.tableMetadata.filter,  dateRangeFilter: props.filter.value, startDate: startDateValue, endDate: endDateValue }),
                });

                if (response.status === 204) {
                    setData([]);
                } else {
                    const data = await response.json();
                    setData(data.healthChecks);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        if (props.tableMetadata.ids && props.tableMetadata.ids.length > 0) {
            fetchData(url, props.tableMetadata.ids);
        }
        else {           
            fetchData(url, []);
        }        
    }, [url, props.tableMetadata.filter]);

    const handleRestartRouterService = async (routerType, routerAddress) => {

        let action = { type: routerType, actionId: routerAddress };


        setRequestedAction(action);

        try {
            const response = await fetch("api/router/restart",
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ address: routerAddress })
                });


            if (response.status != 200) {
                setRestartError("Restart router not working");
            } else {
                const data = await response.json();
                if (!data.success) {
                    setRestartError("Restart router not working");
                }
            }           
        } catch (error) {
            throw error;
        }

        setRequestedAction();
    }

    if (data && data.length > 0) {
        tableHeader = [
            { label: "Id", key: "id" },
            { label: "Tenant Name", key: "tenantName" },
            { label: "Router Address", key: "routerAdress" },
            { label: "Status", key: "status" },
            { label: "Date", key: "statusDate" },
            { label: "Previous Status", key: "previousStatus" },
            { label: "Date", key: "previousStatusDate" },
            { label: "Last Communication", key: "lastCommunication" },
            { label: "Last Reboot", key: "lastReboot" },
        ];

        tableData = data.map((prop, key) => {
            let actionButtons = (
                <>                    
                    <Button
                        onClick={(e) => handleRestartRouterService(prop.RouterType, prop.routerAdress)}
                        title="Restart Router Service"
                        variant="primary"
                        size="sm"
                        className="btn-primary edit"
                    >
                        Restart
                    </Button>
                </>
            );

            if (requestedAction && requestedAction.actionId == prop.routerAdress) {
                actionButtons = <Spinner animation="border" variant="primary" />;
            }

            return {
                id: prop.id,
                tenantName: prop.tenantName,
                routerAdress: prop.routerAdress,                
                RouterType: prop.RouterType,
                status: prop.status,
                statusDate: (prop.modified == null) ? "" : moment.utc(prop.modified).local().format("MM/DD/YYYY hh:mm:ss A"),
                previousStatus: prop.previousStatus,
                previousStatusDate: (prop.previousStatusDate == null) ? "" : moment.utc(prop.previousStatusDate).local().format("MM/DD/YYYY hh:mm:ss A"),
                lastCommunication: (prop.lastCommunication == null) ? "" : moment.utc(prop.lastCommunication).local().format("MM/DD/YYYY hh:mm:ss A"),
                lastReboot: (prop.lastReboot == null) ? "" : moment.utc(prop.lastReboot).local().format("MM/DD/YYYY hh:mm:ss A"),
                actions: (
                    <div className="actions-right">
                        {actionButtons}
                    </div>
                ),
            };
        });
    }

    table = (
        <ReactTable
            data={tableData}
            columns={[
                {
                    Header: "Id",
                    accessor: "id",
                    className: 'rt-custom',
                    maxWidth: 50,
                    minWidth: 50,
                    width: 60,
                },
                {
                    Header: "Tenant Name",
                    accessor: "tenantName",
                    className: 'rt-first',
                    maxWidth: 200,
                    minWidth: 200,
                    width: 200,
                },
                {
                    Header: "Router Address",
                    accessor: "routerAdress",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 150 ,
                    width: 150,
                },
                {
                    Header: "Status",
                    accessor: "status",
                    className: 'rt-custom',
                    maxWidth: 70,
                    minWidth: 70,
                    width: 70,
                },
                {
                    Header: "Status Date (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "statusDate",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 150,
                    width: 150,
                },
                {
                    Header: "Previous Status",
                    accessor: "previousStatus",
                    className: 'rt-custom',
                    maxWidth: 70,
                    minWidth: 80,

                    width: 70,
                },
                {
                    Header: "Previous Status Date (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "previousStatusDate",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 150,
                    width: 150,
                },
                {
                    Header: "Last Communication (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "lastCommunication",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 150,
                    width: 150,
                },
                {
                    Header: "Last Reboot (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "lastReboot",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 150,
                    width: 150,
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
            className="-striped -highlight primary-pagination"
        />
    );

    return (
        <>
            <Card className="dashboard-table-card">
                <Card.Header className="d-flex justify-content-between">
                    <Card.Title as="h4">{props.title}</Card.Title>
                    <CSVLink
                        data={tableData}
                        headers={tableHeader}
                        filename={"download_file.csv"}
                        className="bg-success btn btn-default btn-sm"
                        target="_blank"
                    >
                        <i className="fas fa-table mr-2"></i>
                        Export to CSV
                     </CSVLink>
                </Card.Header>
                <div className="p-1">
                    <div className="dashboard-controller-table">
                        {table}
                    </div>
                </div>
            </Card>
        </>
    );
}

export default RouterTable;