import React, { useEffect, useState } from 'react';

import ReactTable from './../../../components/Common/ReactTable/ReactTable';

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

const HL7CloudServersPanel = (props) => {
    const [isLoading, setIsLoading] = useState(true);
    const [cloudServers, setCloudServers] = useState([]);
    const [errorMessage, setErrorMessage] = useState();

    let table = undefined;
    let data = [];

    const url = "api/Integration/GetHL7CloudServers";

    const urlHL7ServerUpdate = "api/Integration/Upsert";

    useEffect(() => {
        const fetchData = async (url) => {
            try {
                setIsLoading(true);

                const response = await fetch(url);

                if (response.status === 204) {
                    setCloudServers([]);
                } else {
                    const data = await response.json();
                    setCloudServers(data.hL7CloudServers);
                }

                setIsLoading(false);

            } catch (error) {
                setIsLoading(false);
                throw error;
            }
        };

        fetchData(url);
    }, [url, props.onRefresh]);

    const handleHL7ServerToggleChange =  (e) => {
        const serverId = e.currentTarget.getAttribute("id");
        const activeValue = e.currentTarget.checked;

        let servers = [...cloudServers];

        for (var i = 0; i < servers.length; i++) {
            if (servers[i].id == serverId) {
                servers[i].isActive = activeValue;

                if (updateHL7Server(urlHL7ServerUpdate, servers[i])) {
                    setCloudServers(servers);
                }
                else {
                    setErrorMessage("An Error has occurred.");
                }
                break;
            }
        }       
    }

    const updateHL7Server = async (url, hl7Server) => {
        setIsLoading(true);
        try {

            const response = await fetch(url, {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(hl7Server),
            });

           
            if (response.status != 200) {
                setIsLoading(false);
                return false;
            }

            const data = await response.json();

            setIsLoading(false);
            if (data.Id > 0) {
                return true;
            }

        } catch (error) {
            setIsLoading(false);
            throw error;
        }

        return false;
    }

    if (cloudServers && cloudServers.length > 0) {
        data = cloudServers.map((prop, key) => {
            return {
                id: prop.id,
                tenantName: prop.tenantName,
                facilityName: prop.facilityName,
                cloudServerId: prop.cloudServerId,
                cloudServerAddress: prop.cloudServerAddress,
                port: prop.port,
                maxPacketsPerMessage: prop.maxPacketsPerMessage,
                enable: (
                    <div className="actions-right">
                        <Form.Check
                            type="switch"
                            id={prop.id}
                            className="mb-1"
                            checked={prop.isActive}
                            onChange={handleHL7ServerToggleChange}
                        />{" "}
                        {/*<Button*/}
                        {/*    variant="danger"*/}
                        {/*    size="sm"*/}
                        {/*    className="btn-link remove text-danger"*/}
                        {/*>*/}
                        {/*    <i className="fa fa-times" />*/}
                        {/*</Button>{" "}*/}
                    </div>
                ),
            };
        });
    }

    table = (
        <ReactTable
            data={data}
            columns={[
                {
                    Header: "Id",
                    accessor: "id",
                    className: 'rt-first',
                    maxWidth: 100,
                    minWidth: 50,
                    width: 50,
                },
                {
                    Header: "Tenant Name",
                    accessor: "tenantName",
                    className: 'rt-custom',
                    maxWidth: 320,
                    minWidth: 320,
                    width: 320,
                },
                {
                    Header: "Facility Name",
                    accessor: "facilityName",
                    className: 'rt-custom',
                    maxWidth: 320,
                    minWidth: 320,
                    width: 320,
                },
                {
                    Header: "Cloud Server Id",
                    accessor: "cloudServerId",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 80,
                    width: 80,
                },
                {
                    Header: "Cloud Server Address",
                    accessor: "cloudServerAddress",
                    className: 'rt-custom',
                    maxWidth: 320,
                    minWidth: 320,
                    width: 320,
                },
                {
                    Header: "Port",
                    accessor: "port",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 80,
                    width: 80,
                },
                {
                    Header: "Max Packets Per Message",
                    accessor: "maxPacketsPerMessage",
                    className: 'rt-custom',
                    maxWidth: 100,
                    minWidth: 80,
                    width: 80,
                },
                {
                    Header: "Enable",
                    accessor: "enable",
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
            <Card.Body className="table-full-width">
                <Row>
                    <Col className="mx-auto setup-facilities-table pt-2">
                        {table}
                    </Col>
                </Row>
            </Card.Body>
        </>
    );
}

export default HL7CloudServersPanel;
