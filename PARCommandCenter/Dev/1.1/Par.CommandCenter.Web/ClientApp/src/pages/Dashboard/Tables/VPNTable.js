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


const VPNTable = (props) => {

    const [isLoading, setIsLoading] = useState(false);
    const [data, setData] = useState();

    let startDateValue = "", endDateValue = "";
    if (props.filter.startDate && props.filter.endDate) {
        startDateValue = props.filter.startDate.format("MM/DD/YYYY");
        endDateValue = props.filter.endDate.format("MM/DD/YYYY");
    }

    const url = props.tableMetadata.dataUrl;

    let vpnTable = undefined;
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
                    body: JSON.stringify({ healthCheckIds: ids, filter: props.tableMetadata.filter, dateRangeFilter: props.filter.value, startDate: startDateValue, endDate: endDateValue }),
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

    if (data && data.length > 0) {
        tableHeader = [
            { label: "Id", key: "id" },
            { label: "Tenant Name", key: "tenantName" },
            { label: "Facility Name", key: "facilityName" },
            { label: "Connection Name", key: "connectionName" },
            { label: "Status", key: "status" },
            { label: "Date", key: "statusDate" },
            { label: "Previous Status", key: "previousStatus" },
            { label: "Date", key: "previousStatusDate" }
        ];

        tableData = data.map((prop, key) => {
            return {
                id: prop.id,
                tenantName: (prop.tenant) ? prop.tenant.name : "",
                facilityName: (prop.facilities && prop.facilities.length > 0) ? prop.facilities.map((item) => { return item.name }).join(" | ") : "",
                connectionName: prop.connectionName,
                status: prop.status,
                statusDate: (prop.modified == null) ? "" : moment.utc(prop.modified).local().format("MM/DD/YYYY hh:mm:ss A"),
                previousStatus: prop.previousStatus,
                previousStatusDate: (prop.previousStatusDate == null) ? "" : moment.utc(prop.previousStatusDate).local().format("MM/DD/YYYY hh:mm:ss A"),
            };
        });
    }
    

    vpnTable = (
        <ReactTable
            data={tableData}
            columns={[
                {
                    Header: "Id",
                    accessor: "id",
                    className: 'rt-custom',
                    maxWidth: 30,
                    minWidth: 30,
                    width: 30,
                },
                {
                    Header: "Tenant Name",
                    accessor: "tenantName",
                    className: 'rt-first-left',
                    maxWidth: 200,
                    minWidth: 70,
                    width: 200,
                },
                {
                    Header: "Facility Name",
                    accessor: "facilityName",
                    className: 'rt-first-left',
                    maxWidth: 200,
                    minWidth: 70,
                    width: 200,
                },
                {
                    Header: "Connection Name",
                    accessor: "connectionName",
                    className: 'rt-first-left',
                    maxWidth: 400,
                    minWidth: 300,
                    width: 320,
                },
                {
                    Header: "Status",
                    accessor: "status",
                    className: 'rt-custom',
                    maxWidth: 70,
                    minWidth: 70,
                    width: 200,
                },
                {
                    Header: "Status Date (" + moment().tz(moment.tz.guess()).zoneAbbr() + ")",
                    accessor: "statusDate",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 150,
                    width: 150,
                },
                //{
                //    Header: "Previous Status",
                //    accessor: "previousStatus",
                //    className: 'rt-custom',
                //    maxWidth: 70,
                //    minWidth: 70,

                //    width: 70,
                //},
                //{
                //    Header: "Previous Status Date",
                //    accessor: "previousStatusDate",
                //    className: 'rt-custom',
                //    maxWidth: 200,
                //    minWidth: 150,
                //    width: 150,
                //},
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
                        {vpnTable}
                    </div>
                </div>            
            </Card>
        </>
    );
}

export default VPNTable;