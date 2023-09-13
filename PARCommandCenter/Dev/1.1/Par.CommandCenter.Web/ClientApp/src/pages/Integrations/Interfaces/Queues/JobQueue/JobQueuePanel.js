﻿import React, { useEffect, useState } from 'react';

import ReactTable from './../../../../../components/Common/ReactTable/ReactTable';

import SweetAlert from 'react-bootstrap-sweetalert';

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

import moment from "moment-timezone";

const JobQueuePanel = (props) => {

    const [alert, setAlert] = useState();
    const [doRefresh, setDoReferesh] = useState(false);
    const [isLoading, setIsLoading] = useState(true);


    const [jobQueueItems, setJobQueueItems] = useState([]);

    let JobQueueTable = undefined;
    
   
    const url = "api/JobQueue/GetByTenantId/" + props.tenant.id;

    useEffect(() => {
        const fetchData = async (url) => {
            try {
                setIsLoading(true);
                
                const response = await fetch(url);

                if (response.status === 204) {
                    setJobQueueItems([]);
                } else {
                    const data = await response.json();
                    setJobQueueItems(data.jobQueueItems);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        fetchData(url);

    }, [url, doRefresh]);

    const handleRestartJob = async (job) => {        
        try {
            const response = await fetch("api/jobqueue/reset",
                {
                    method: "post",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ id: job.id })
                }
            );

            hideAlert();

            if (response.status === 400) {
                let responseBody = await response.json();
                console.log(responseBody);
            } else if (response.status === 200) {
                let responseBody = await response.json();
                
                ////if (responseBody.success) {
                ////    setDoReferesh(!doRefresh);
                ////}
                setDoReferesh(!doRefresh);
            }
        } catch (e) {
            console.log(e);
        }
    }

    const handleDeleteJob = async (job) => {
        try {
            const response = await fetch("api/jobqueue/delete",
                {
                    method: "delete",
                    headers: new Headers({
                        "Content-Type": "application/json",
                        Accept: "application/json"
                    }),
                    body: JSON.stringify({ id: job.id })
                }
            );

            hideAlert();

            if (response.status === 400) {
                let responseBody = await response.json();
                console.log(responseBody);
            } else if (response.status === 200) {
                let responseBody = await response.json();

                ////if (responseBody.success) {
                ////    setDoReferesh(!doRefresh);
                ////}                
            }
            setDoReferesh(!doRefresh);
        } catch (e) {
            console.log(e);
        }
    }

    const confirmDeleteJobMessage = (job) => {
        setAlert(
            <SweetAlert
                style={{ display: "block", marginTop: "-100px" }}
                title='Are you sure?'
                onConfirm={() => handleDeleteJob(job)}
                onCancel={() => hideAlert()}
                confirmBtnBsStyle=" btn-medium btn-danger"
                cancelBtnBsStyle=" btn-medium btn-secondary"
                confirmBtnText="Yes, delete it!"
                cancelBtnText="Cancel"
                showCancel
            >
                This job will be deleted permanently. You can't undo this operation
            </SweetAlert>
        );
    };

    const confirmRestartJobMessage = (job) => {        
        setAlert(
            <SweetAlert
                style={{ display: "block", marginTop: "-100px" }}
                title='Are you sure?'
                onConfirm={() => handleRestartJob(job)}
                onCancel={() => hideAlert()}
                confirmBtnBsStyle=" btn-medium btn-danger"
                cancelBtnBsStyle=" btn-medium btn-secondary"
                confirmBtnText="Yes, reset it!"
                cancelBtnText="Cancel"
                showCancel
            >
                This job will be reset. You can't undo this operation
            </SweetAlert>
        );
    };

    const hideAlert = () => {
        setAlert(null);
    };

    let data = [];
    if (jobQueueItems && jobQueueItems.length > 0) {
        data = jobQueueItems.map((prop, key) => {            
            return {
                submitted: (prop.submitted) ? moment.utc(prop.submitted).local().format("MM/DD/YYYY hh:mm:ss A") : "",
                jobName: (prop.job) ? prop.job.name : "",
                jobType: (prop.jobType) ? prop.jobType.name : "",
                runOnceDate: (prop.runOnceDate) ? moment.utc(prop.runOnceDate).local().format("MM/DD/YYYY hh:mm:ss A") : "",
                started: (prop.started) ? moment.utc(prop.started).local().format("MM/DD/YYYY hh:mm:ss A") : "",
                errorMessage: prop.errorMessage,
                actions: (
                    <div className="actions-right">
                        <Button                            
                            variant="primary"
                            size="sm"
                            className="text-warning btn-link edit"
                            onClick={() => { confirmRestartJobMessage(prop);}}
                        >
                            <i className="fa fa-undo" />
                        </Button>{" "}
                        <Button
                            variant="danger"
                            size="sm"
                            className="btn-link remove text-danger"
                            onClick={() => { confirmDeleteJobMessage(prop); }}
                        >
                            <i className="fa fa-times" />
                        </Button>{" "}
                    </div>
                ),
            };
        });
    }

    JobQueueTable = (
        <ReactTable
            data={data}
            columns={[
                {
                    Header: "Submitted",
                    accessor: "submitted",
                    className: 'rt-custom',
                    maxWidth: 150,
                    minWidth: 150,
                    width: 150,
                },
                {
                    Header: "Job Name",
                    accessor: "jobName",
                    className: 'rt-custom',
                    maxWidth: 150,
                    minWidth: 150,
                    //width: 140,
                },
                {
                    Header: "Job Type",
                    accessor: "jobType",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 100,
                    width: 100,
                },
                {
                    Header: "Run Once Date",
                    accessor: "runOnceDate",
                    className: 'rt-custom',
                    maxWidth: 150,
                    minWidth: 150,
                    width: 150,
                },
                {
                    Header: "Started",
                    accessor: "started",
                    className: 'rt-custom',
                    maxWidth: 150,
                    minWidth: 70,

                    width: 70,
                },
                {
                    Header: "Error",
                    accessor: "errorMessage",
                    className: 'rt-custom',
                    maxWidth: 200,
                    minWidth: 200,
                    width: 200,
                },
                {
                    Header: "Actions",
                    accessor: "actions",
                    className: 'rt-custom',
                    sortable: false,
                    filterable: false,
                    maxWidth: 100,
                    minWidth: 100,

                    width: 100,
                },
            ]}
            /*
                You can choose between primary-pagination, info-pagination, success-pagination, warning-pagination, danger-pagination or none - which will make the pagination buttons gray
            */
            className="-striped -highlight primary-pagination"
        />
    );

    return (       
  
            <Row>
                <Col className="mx-auto queues-table pt-3">
                    {JobQueueTable}
                </Col>
            `   {alert}
            </Row>
    );
}

export default JobQueuePanel;