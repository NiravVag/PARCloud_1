import React, { useEffect, useState } from 'react';

import  Carousel from "react-multi-carousel";
import "react-multi-carousel/lib/styles.css";

import { useAppState } from "./../../state";

import { useLocation } from "react-router-dom";

// react-bootstrap components
import {
    Badge,
    Button,
    Card,
    Collapse,
    Form,
    InputGroup,
    Navbar,
    Nav,
    OverlayTrigger,
    Table,
    Tooltip,
    Container,
    Row,
    Col,
    Modal
} from "react-bootstrap";

import DashboardCard from "./DashboardCard";
import cardsData from "./DashboardCardsData";
import VPNTable from './Tables/VPNTable';
import InterfaceTable from './Tables/InterfaceTable';
import RouterTable from './Tables/RouterTable';
import ControllerTable from './Tables/Controller/ControllerTable';
import ScaleTable from './Tables/ScaleTable';

import "./DashboardPage.scss";
import FilterList from '../../components/Common/FilterList';
import ErrorFilter from './ErrorFilter';
import TenantsScaleMeasurementChart2 from './TenantsScaleMeasurementChart2';
import AppHostingInfoCard from './AppHostingInfoCard';


function DashboardPage(props) {

    const [environment, setEnvironment] = useState();

    const [isLoading, setIsloading] = useState(false); 
    
    const [hostingCardStatistics, setHostingCardStatistics] = useState();

    const { user, app, onAppStateRefresh } = useAppState();
    
    const [cardsInfo, setCardsInfo] = useState();

    const [additionalInfo, setAdditionalInfo] = useState();    
    
    const [showTable, setShowTable] = useState(false);

    const [cardsStatisticData, setCardsStatisticData] = useState({});

    const [selectedFilter, setSelectedFilter] = useState({ value: "past24Hours", label: "Past 24 hours" });

    const [errorFilterSelected, setErrorFilterSelected] = useState(true);

    const [showModal, setShowModal] = useState(false);
    const [modalTitle, setModalTitle] = useState("");
    const [modalContent, setModalContent] = useState();

    const queryParameters = new URLSearchParams(window.location.search)
    const timeRangeFilter = queryParameters.get("filter")
    //console.log(timeRangeFilter);

    const handleModalClose = () => {
        setShowModal(false);
    }

    const handleModalOpen = (modalHeader, modalContent) =>
    {        
        setModalTitle(modalHeader);
        setModalContent(modalContent);
        setShowModal(!showModal);
    }

    const handleCancelErrorFilter = () => {
        setErrorFilterSelected(false);
    }

    const responsive = {
        superLargeDesktop: {
            // the naming can be any, depends on you.
            breakpoint: { max: 4000, min: 3000 },
            items: 5
        },
        desktop: {
            breakpoint: { max: 3000, min: 1700 },
            items: 5
        },
        smallDesktp: {
            breakpoint: { max: 1700, min: 1366 },
            items: 4
        },
        tabletLarge: {
            breakpoint: { max: 1366, min: 1200 },
            items: 3
        },
        tablet: {
            breakpoint: { max: 1200, min: 700 },
            items: 2
        },
        mobile: {
            breakpoint: { max: 700, min: 0 },
            items: 1
        }
    };

    useEffect(() => {
        if (cardsData && cardsData.length > 0) {
            setCardsInfo(cardsData);           
        }
    }, [cardsData]);

    useEffect(() => { 
        fetch("api/Application/HostingEnvironment", {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
            /*body: JSON.stringify(props.user.tenantIds)*/
        })
            .then(response => response.json())
            .then(data => {
                setEnvironment(data.environment);
            });
    }, []); 

    useEffect(() => {        
        setIsloading(true);        
        fetch("api/Application/HostingInfo", {
            method: 'GET',
            headers: {
                'Content-Type': 'application/json',
            },
            /*body: JSON.stringify(props.user.tenantIds)*/
        })
            .then(response => response.json())
            .then(data => {
                setHostingCardStatistics(data);
                setIsloading(false);
            });        
    }, [environment]); 


    const handleShowAdditionalClick = (additional) => {
        if (additional.dataUrl.length > 0) {
            setAdditionalInfo(additional);         

            setShowTable(true);
        }
    }    

    const handleDataBackFromCard = (myData) => {
        let hash = cardsStatisticData;
        hash[myData.type] = myData;
        setCardsStatisticData(hash);
    }

    const handleDashboardFilter = (filter) => {
        if ((filter.value != "customDate") || (filter.value == "customDate" && filter.startDate && filter.endDate)) {
            setSelectedFilter(filter);
        }        
    }

    if (!additionalInfo && props.location.search) {
        let ids = [];

        if (props.location.search.indexOf("ids") > 0) {
            let idsIndex = props.location.search.indexOf("ids=") + 4;
            let idsString = props.location.search.substring(idsIndex);

            idsString.split(',').forEach(function (item) {
                ids.push(parseInt(item))
            });
        }


        if ((props.location.search.indexOf("vpns") > 0 && !additionalInfo) || (additionalInfo && additionalInfo.type != "vpns" && props.location.search.indexOf("vpns") > 0)) {
            let memberName = "vpns"
            if (cardsInfo && cardsInfo.length > 0) {
                let member = cardsInfo.find(m => m.headerName.toLowerCase() == memberName);

                setAdditionalInfo({ids: ids, type: member.headerName.toLowerCase(), title: member.headerName, dataUrl: member.errorDataUrl, filter: "errors", dataTableTitle: member.headerName, dataTableSubTitle: member.cardTitleDanger });
                setShowTable(true);
            }
        } else if ((props.location.search.indexOf("routers") > 0 && !additionalInfo) || (additionalInfo && additionalInfo.type != "routers" && props.location.search.indexOf("routers") > 0)) {
            let memberName = "routers"
            if (cardsInfo && cardsInfo.length > 0) {
                let member = cardsInfo.find(m => m.headerName.toLowerCase() == memberName);
                

                setAdditionalInfo({ ids: ids, type: member.headerName.toLowerCase(), title: member.headerName, dataUrl: member.errorDataUrl, filter: "errors", dataTableTitle: member.headerName, dataTableSubTitle: member.cardTitleDanger });
                setShowTable(true);
            }
        }
        else if ((props.location.search.indexOf("controllers") > 0 && !additionalInfo) || (additionalInfo && additionalInfo.type != "controllers" && props.location.search.indexOf("controllers") > 0)) {
            let memberName = "controllers"
            if (cardsInfo && cardsInfo.length > 0) {
                let member = cardsInfo.find(m => m.headerName.toLowerCase() == memberName);

                setAdditionalInfo({ ids: ids, type: member.headerName.toLowerCase(), title: member.headerName, dataUrl: member.errorDataUrl, filter: "errors", dataTableTitle: member.headerName, dataTableSubTitle: member.cardTitleDanger });
                setShowTable(true);
            }
        }
        else if ((props.location.search.indexOf("interfaces") > 0 && !additionalInfo) || (additionalInfo && additionalInfo.type != "interfaces" && props.location.search.indexOf("interfaces") > 0)) {
            let memberName = "interfaces"
            if (cardsInfo && cardsInfo.length > 0) {
                let member = cardsInfo.find(m => m.headerName.toLowerCase() == memberName);

                setAdditionalInfo({ ids: ids, type: member.headerName.toLowerCase(), title: member.headerName, dataUrl: member.errorDataUrl, filter: "errors", dataTableTitle: member.headerName, dataTableSubTitle: member.cardTitleDanger });
                setShowTable(true);
            }
        }
    }

    let additionalTable;
    if (additionalInfo) {
        let requestedTable;
        requestedTable = additionalInfo.type;

        switch (requestedTable) {
            case "vpns":
                additionalTable = (<VPNTable title={additionalInfo.title} tableMetadata={additionalInfo} user={user} filter={selectedFilter} />);
                break;
            case "interfaces":
                additionalTable = (<InterfaceTable title={additionalInfo.title} tableMetadata={additionalInfo} user={user} filter={selectedFilter} />);
                break;
            case "routers":
                additionalTable = (<RouterTable title={additionalInfo.title} tableMetadata={additionalInfo} user={user} filter={selectedFilter}/>);
                break;
            case "controllers":
                additionalTable = (<ControllerTable title={additionalInfo.title} tableMetadata={additionalInfo} user={user} filter={selectedFilter}
                    onOpenModal={handleModalOpen} onCloseModal={handleModalClose} />
                );
                break;
            case "scales":
                additionalTable = (<ScaleTable title={additionalInfo.title} tableMetadata={additionalInfo} user={user} filter={selectedFilter}/>);
                break;
            default:
                break;
        }
    }

    let cardsContent, carouselContent;
    if (cardsInfo && cardsInfo.length > 0) {
        cardsContent = cardsInfo.map((member, index) => {
            let isCardSelected = false;
            if (additionalInfo && additionalInfo.type == member.headerName.toLowerCase()) {
                isCardSelected = true;
            }

            if (member.isSpecialType) {                
                if (environment && member.cardComponentName === "AppHostingInfoCard") {
                    return (
                        <AppHostingInfoCard key={index}
                            icon={member.icon}
                            headerName={member.headerName}
                            cardDataUrl={member.cardDataUrl}
                            isDevOnly={member.isDevOnly}
                            isLoading={isLoading}
                            cardStatistics={hostingCardStatistics}
                            environment={environment}
                        />
                    );
                }
            }

            return (
                <DashboardCard key={index} cardIndex={index}
                    user={user}
                    icon={member.icon}
                    headerName={member.headerName}
                    cardTitleDanger={member.cardTitleDanger}
                    cardTitleWarning={member.cardTitleWarning}
                    cardTitleActive={member.cardTitleActive}
                    cardCountsUrl={member.cardCountsUrl}

                    errorDataUrl={member.errorDataUrl}
                    warningsDataUrl={member.warningsDataUrl}
                    activeDataUrl={member.activeDataUrl}
                    showAdditionalClick={handleShowAdditionalClick}
                    selected={isCardSelected}
                    dataCallBack={handleDataBackFromCard}
                    cardStatisticData={(cardsStatisticData) ? cardsStatisticData[member.headerName.toLowerCase()] : undefined}

                    ftpServerStatusUrl={member.ftpServerStatusUrl}

                    filter={selectedFilter}

                    errorFilter={errorFilterSelected}
                />
            );
        }
        );

        carouselContent = (
            <Carousel responsive={responsive} infinite={true} swipeable={true} removeArrowOnDeviceType={["superLargeDesktop"]} >
                {cardsContent}
            </Carousel>
        );
    }

    return (
        <>
            <Container fluid id="dashboard">
                
                        <div className="col-md-6" >
                            <div className="d-flex " >
                        <FilterList onSelection={handleDashboardFilter} />

                        <ErrorFilter selected={errorFilterSelected} onCancel={handleCancelErrorFilter}/>
                    </div>
                </div>
                
                {carouselContent}


                {showTable && additionalTable

                }               
                <TenantsScaleMeasurementChart2 />
                <Modal
                    size="lg"
                    show={showModal}
                    onHide={handleModalClose}
                    centered
                >
                    <Modal.Header closeButton>
                        <h4 className="modal-title text-center">
                            {modalTitle}
                        </h4>

                    </Modal.Header>
                    <Modal.Body>
                        {modalContent}
                    </Modal.Body>
                </Modal>
            </Container>
        </>
    );
}




export default DashboardPage;






