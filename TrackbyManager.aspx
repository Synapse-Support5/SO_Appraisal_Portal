<%@ Page Title="TrackbyManager" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="TrackbyManager.aspx.cs" Inherits="SO_Appraisal.TrackbyManager" %>

<asp:Content ID="HeadContent" ContentPlaceHolderID="HeadContent" runat="server">
    <link href="Content/bootstrap.css" rel="stylesheet" />
    <link rel="stylesheet" href="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/css/bootstrap.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/css/bootstrap-datepicker.min.css" />
    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/select2@4.1.0-rc.0/dist/css/select2.min.css" />

    <link rel="stylesheet" href="https://cdn.jsdelivr.net/npm/bootstrap-icons@1.10.5/font/bootstrap-icons.css">

    <%--    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.4.2/css/all.min.css" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-icons/1.10.5/font/bootstrap-icons.min.css" />--%>

    <style>
        /* Solid CSS so styles work even with CDN Tailwind (no @apply needed) */
        .card {
            display: flex;
            flex-direction: column;
            align-items: center;
            justify-content: center;
            padding: 1.5rem;
            background: #fff;
            border-radius: 0.5rem;
            box-shadow: 0 1px 2px rgba(0,0,0,.05), 0 1px 1px rgba(0,0,0,.04);
            transition: background-color .2s ease, box-shadow .2s ease, transform .1s ease;
            text-align: center;
            font-weight: 600;
            color: #374151;
        }

            .card:hover {
                box-shadow: 0 10px 15px rgba(0,0,0,.1), 0 4px 6px rgba(0,0,0,.05);
                transform: translateY(-2px);
            }

        .icon {
            width: 40px;
            height: 40px;
            margin-bottom: 0.75rem;
        }

        .muted {
            color: #6b7280;
        }

        .animate-fade-in {
            animation: fadeIn 0.3s ease-out;
        }

        @keyframes fadeIn {
            from {
                opacity: 0;
                transform: translateY(20px);
            }

            to {
                opacity: 1;
                transform: translateY(0);
            }
        }

        .toast-custom {
            position: fixed;
            top: 10px;
            right: 20px;
            width: 300px;
            background-color: #fff;
            border-radius: 5px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            padding: 15px;
        }

        .toast-success {
            border-left: 5px solid #28a745; /* Light green */
        }

        .toast-danger {
            border-left: 5px solid #dc3545; /* Red */
        }

        .form-control {
            text-align: center;
        }

        @media (max-width: 768px) {
            .col-12 {
                text-align: center;
            }

            /*#btnOpenModal {
                width: 100%;
            }
*/
            .grid-wrapper {
                overflow-x: auto;
                -webkit-overflow-scrolling: touch;
            }

                .grid-wrapper table {
                    width: 100%;
                    display: block;
                }

                    .grid-wrapper table thead,
                    .grid-wrapper table tbody {
                        display: table;
                        width: 100%;
                    }
        }

        .navbar-white .navbar-toggler {
            border-color: rgba(0, 0, 0, 0.1);
        }

        .navbar-white .navbar-toggler-icon {
            background-image: url("data:image/svg+xml;charset=utf8,%3Csvg viewBox='0 0 30 30' xmlns='http://www.w3.org/2000/svg'%3E%3Cpath stroke='rgba(0, 0, 0, 0.5)' stroke-width='2' stroke-linecap='round' stroke-miterlimit='10' d='M4 7h22M4 15h22M4 23h22'/%3E%3C/svg%3E");
        }

        .grid-wrapper {
            max-height: 300px;
            max-width: 100%;
            overflow: auto;
        }

        /* Keyframes for progress animation */
        @keyframes progress-animation {
            0% {
                width: 0%;
            }

            50% {
                width: 50%;
            }

            100% {
                width: 100%;
            }
        }

        /* Adjustments for small screens */
        @media (max-width: 768px) {
            .progress-bar-container {
                width: 100%; /* Full width on smaller screens */
                max-width: 100%; /* Ensures the progress bar can stretch to the screen size */
            }
        }

        /* Style the autocomplete dropdown to look like a DropDownList */
        .ui-autocomplete {
            list-style: none;
            margin: 0;
            padding: 0;
            background-color: white;
            border: 1px solid #ccc;
            border-radius: 4px;
            max-height: 200px;
            overflow-y: auto;
            box-shadow: 0 2px 6px rgba(0, 0, 0, 0.1);
            z-index: 1050;
            width: 50px;
        }

        .ui-menu-item {
            padding: 8px 12px;
            font-size: 14px;
        }

            .ui-menu-item:hover {
                background-color: #007bff;
                color: white;
            }

        /* Hide any default message shown by jQuery UI */
        .ui-helper-hidden-accessible {
            display: none;
        }

        /* Common alert styles */
        .alert-box {
            display: none;
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 1050; /* Ensure it stays on top */
            max-width: 500px;
            width: 100%;
            background-color: white;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
            border-radius: 5px;
            padding: 20px;
        }

        /* Common alert backdrop */
        .alert-backdrop {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background: rgba(0, 0, 0, 0.5);
            z-index: 1049; /* Just below the alert */
        }

        .loader-container {
            display: none; /* Initially hidden */
            position: fixed;
            top: 50%;
            left: 50%;
            transform: translate(-50%, -50%);
            z-index: 1051; /* Ensure it stays on top */
            flex-direction: column;
            align-items: center;
        }

        .loader {
            width: 80px;
            height: 80px;
            display: flex;
            align-items: center;
            justify-content: center;
            position: relative;
            margin-bottom: 20px;
        }

            .loader::before, .loader::after {
                content: "";
                position: absolute;
                border-radius: 50%;
                animation: pulse 1.5s infinite ease-in-out;
            }

            .loader::before {
                width: 80px;
                height: 80px;
                border: 6px solid #ff7eb3;
            }

            .loader::after {
                width: 60px;
                height: 60px;
                border: 6px solid #ff758c;
                animation-delay: 0.75s;
            }

        @keyframes pulse {
            0% {
                transform: scale(1);
                opacity: 1;
            }

            50% {
                transform: scale(1.3);
                opacity: 0.5;
            }

            100% {
                transform: scale(1);
                opacity: 1;
            }
        }

        .loading-text {
            font-size: 18px;
            color: #333;
            font-weight: bold;
            text-align: center;
        }

        /*Floating Label*/
        .floating-label {
            position: relative;
            /* margin-top: 1.5rem; */
            /*margin-bottom: 1.5rem;*/
        }

            .floating-label label {
                position: absolute;
                top: -0.6rem;
                left: 0.75rem;
                background: white;
                padding: 0 4px;
                font-size: 0.8rem;
                color: #3f51b5;
                z-index: 1;
            }

            .floating-label select,
            .floating-label input,
            .floating-label button,
            .floating-label .aspNetDropDown {
                width: 100%;
                /* padding: 0.6rem 0.75rem; */
                font-size: 1rem;
                border: 1px solid #ccc;
                border-radius: 4px;
                background-color: transparent;
                /*appearance: none;*/
                /*padding: 0.6rem 0.75rem;*/
                position: relative;
                z-index: 0;
            }

            .floating-label button {
                text-align: left;
            }

            /* Optional: prevent pointer cursor on readonly textbox */
            .floating-label input[readonly] {
                background-color: #f9f9f9;
                cursor: default;
            }

            .floating-label select:focus,
            .floating-label select:not([value=""]) {
                outline: none;
            }

                .floating-label select:focus + label,
                .floating-label select:not([value=""]) + label {
                    top: -0.6rem;
                    font-size: 0.8rem;
                    color: #3f51b5;
                }

        .disabled-red {
            outline: 2px solid red !important;
            box-shadow: 0 0 5px red !important;
            cursor: not-allowed !important;
        }

        /* OUTER WRAPPER CARD */
        .dashboard-wrapper {
            background: #dce3ea; /*#f8f9fa*/
            border-radius: 10px;
            padding: 25px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.08);
        }

        /* INNER DASHBOARD CARD */
        .dashboard-card {
            background: #ffffff;
            border-radius: 8px;
            padding: 18px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.06);
            height: 100%;
        }

        /* Title smaller */
        .dashboard-card-title {
            font-size: 14px;
            font-weight: 600;
            margin-bottom: 12px;
            text-align: center;
            color: #374151;
        }

        /* Table font smaller */
        .dashboard-card table {
            font-size: 12px;
        }

        /* Spacing between cards */
        .dashboard-row > div {
            margin-bottom: 20px;
        }

        .rating-stars {
            display: flex;
            gap: 6px;
        }

            .rating-stars i {
                font-size: 26px;
                cursor: pointer;
                transition: transform 0.15s ease;
            }

                .rating-stars i:hover {
                    transform: scale(1.2);
                }

        .rating-group.form-control {
            display: flex;
            align-items: center;
            gap: 6px;
            height: auto;
            padding: 6px 10px;
        }

        .kca-card {
            background: #ffffff;
            border-radius: 10px;
            box-shadow: 0 2px 8px rgba(0,0,0,0.06);
            /*transition: all 0.2s ease;*/
        }

            .kca-card:hover {
                /*transform: translateY(-2px);*/
                /*box-shadow: 0 6px 14px rgba(0,0,0,0.1);*/
            }

        /* Old rating (grey) */
        .old-stars i {
            color: #adb5bd;
        }

        /* New rating highlight */
        .new-rating .rating-stars i {
            font-size: 22px;
        }

        /* Difference colors */
        .text-improved {
            color: #28a745;
        }

        .text-reduced {
            color: #dc3545;
        }

        .text-same {
            color: #6c757d;
        }

        .rating-value {
            font-size: 13px;
            font-weight: 600;
        }

        .old-value {
            color: #6c757d;
        }

        .new-rating .rating-value {
            color: #28a745;
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>
    <%--<script src="https://cdn.tailwindcss.com"></script>--%>
    <!-- Lucide icons -->
    <script src="https://cdn.jsdelivr.net/npm/lucide@0.469.0/dist/umd/lucide.min.js"></script>

    <script>
        function showToast(message, styleClass) {
            var toast = $('<div class="toast-custom ' + styleClass + '">' + message + '</div>').appendTo('#toastContainer');

            // Show the toast
            toast.fadeIn();

            // Move existing toasts down
            $('.toast-custom').not(toast).each(function () {
                $(this).animate({ top: "+=" + (toast.outerHeight() + 10) }, 'fast');
            });

            // Hide the toast after 3 seconds
            setTimeout(function () {
                toast.fadeOut(function () {
                    // Remove the toast from DOM after fadeOut
                    $(this).remove();

                    // Move remaining toasts up
                    $('.toast-custom').each(function (index) {
                        $(this).animate({ top: "-=" + (toast.outerHeight() + 10) }, 'fast');
                    });
                });
            }, 3000);
        }

        function showNoteAlert() {
            $('#noteAlert').show();
        }

        function hideNoteAlert() {
            $('#noteAlert').hide();
        }
    </script>

</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="loader-container" id="loaderContainer">
        <div class="loader"></div>
        <div class="loading-text" id="loadingText">Loading...</div>
    </div>

    <div class="container body-content">
        <h2 style="text-align: center; margin-top: 20px;">SO Achivements</h2>

        <div class="headtag">
            <asp:Label ID="lblUserName" runat="server" Style="color: black; float: right; margin-top: 0px; margin-bottom: -20px; margin-right: 20px"></asp:Label>
        </div>

        <br />
        <br />

        <div class="container">
            <div class="row justify-content-end">
                <div class="col-md-2 col-6 mb-2">
                    <asp:Button ID="PendingApprovalsBtn"
                        runat="server"
                        Text="Pending Approvals"
                        CssClass="btn btn-outline-primary form-control" OnClick="PendingApprovalsBtn_Click" />
                </div>

                <div class="col-md-2 col-6 mb-2">
                    <asp:Button ID="ViewAllBtn"
                        runat="server"
                        Text="View All"
                        CssClass="btn btn-outline-primary form-control" OnClick="ViewAllBtn_Click" />
                </div>
            </div>

            <div id="PendApprovalsSec" runat="server" visible="false">
                <div class="row mt-3">
                    <div class="col-12">
                        <div class="grid-wrapper">
                            <asp:Label ID="GridStatusLabel" runat="server"></asp:Label>
                            <asp:GridView ID="PendingApprovalsGrid" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                                Style="margin-bottom: 0px; text-align: center; font-size: small;" DataKeyNames="RequestId" OnRowDataBound="PendingApprovalsGrid_RowDataBound">
                                <Columns>
                                    <asp:TemplateField HeaderText="Select All">
                                        <HeaderTemplate>
                                            <div style="margin-right: 14px; position: relative; align-items: center; align-content: center;">
                                                <input type="checkbox" id="parentCheckbox" style="margin-left: 0px;" class="form-check-input" />
                                            </div>
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            <div style="margin-right: 14px; position: relative;">
                                                <input type="checkbox" id="CheckBox1" runat="server" class="child-checkbox form-check-input rowCheckbox" style="margin-left: 0px;" />
                                            </div>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:BoundField DataField="RequestId" HeaderText="RequestId" />
                                    <asp:BoundField DataField="SOCode" HeaderText="SOCode" />
                                    <asp:BoundField DataField="Quarter" HeaderText="Quarter" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" Visible="false" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="RequestedBy" />
                                    <asp:BoundField DataField="PCYear" HeaderText="PCYear" Visible="false" />

                                    <asp:TemplateField HeaderText="Data">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDownloadThisRequest" runat="server"
                                                CssClass="btn btn-outline-primary ml-1"
                                                CommandArgument='<%# Eval("RequestId") + "," + Eval("SOCode") + "," + Eval("PCYear") + "," + Eval("Quarter") %>'
                                                OnClick="btnDownloadThisRequest_Click"
                                                ToolTip="Download SO requested achievements">
                                                    <i class="bi bi-download"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Objectives">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnViewThisRequest" runat="server"
                                                CssClass="btn btn-outline-info ml-1"
                                                CommandArgument='<%# Eval("RequestId") + "," + Eval("Status") + "," + Eval("SOCode") + "," + Eval("PCYear") + "," + Eval("Quarter") %>'
                                                OnClick="btnViewThisRequest_Click"
                                                OnClientClick="showLoader()"
                                                ToolTip="View this request">
                                        <i class="bi bi-eye"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Approve">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnRowApprove" runat="server"
                                                CssClass="btn btn-outline-success ml-1"
                                                CommandArgument='<%# Eval("RequestId")%>'
                                                OnClientClick='<%# "showApproveAlert(" + Eval("RequestId") + "); return false;" %>'
                                                ToolTip="Approve">
                                        <i class="bi bi-check2-square"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Reject">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnRowReject" runat="server"
                                                CssClass="btn btn-outline-danger ml-1"
                                                CommandArgument='<%# Eval("RequestId")%>'
                                                OnClientClick='<%# "showRejectAlert(" + Eval("RequestId") + "); return false;" %>'
                                                ToolTip="Reject">
                                           <i class="bi bi-x-lg"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

                <div class="row mt-3 justify-content-end" id="ButtonsDiv" runat="server" visible="false">
                    <div class="col-md-2 col-6 mb-2">
                        <asp:Button ID="ApproveSelectedBtn" runat="server" Text="Approve Selected" CssClass="btn btn-outline-success form-control"
                            OnClientClick="approveSelectedAlert(); return false;" />
                    </div>
                    <div class="col-md-2 col-6 mb-2">
                        <asp:Button ID="RejectSelectedBtn" runat="server" Text="Reject Selected" CssClass="btn btn-outline-danger form-control"
                            OnClientClick="approveRejectedAlert(); return false;" />
                    </div>
                </div>
            </div>

            <div id="ViewAllSec" runat="server" visible="false">
                <div class="row mt-3">
                    <div class="col-12">
                        <div class="grid-wrapper">
                            <asp:Label ID="GridStatusLabelViewAll" runat="server"></asp:Label>
                            <asp:GridView ID="ViewAllGrid" runat="server" AutoGenerateColumns="false" CssClass="table table-bordered"
                                Style="margin-bottom: 0px; text-align: center; font-size: small;" DataKeyNames="RequestId">
                                <Columns>
                                    <asp:BoundField DataField="RequestId" HeaderText="RequestId" />
                                    <asp:BoundField DataField="SOCode" HeaderText="SOCode" />
                                    <asp:BoundField DataField="Quarter" HeaderText="Quarter" />
                                    <asp:BoundField DataField="Remarks" HeaderText="Remarks" />
                                    <asp:BoundField DataField="Status" HeaderText="Status" Visible="false" />
                                    <asp:BoundField DataField="Description" HeaderText="Description" />
                                    <asp:BoundField DataField="CreatedOn" HeaderText="CreatedOn" DataFormatString="{0:dd-MMM-yyyy}" HtmlEncode="false" />
                                    <asp:BoundField DataField="CreatedBy" HeaderText="RequestedBy" />
                                    <asp:BoundField DataField="PCYear" HeaderText="PCYear" Visible="false" />

                                    <asp:TemplateField HeaderText="Data">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnDownloadThisRequest" runat="server"
                                                CssClass="btn btn-outline-primary ml-1"
                                                CommandArgument='<%# Eval("RequestId") + "," + Eval("SOCode") + "," + Eval("PCYear") + "," + Eval("Quarter") %>'
                                                OnClick="btnDownloadThisRequest_Click"
                                                ToolTip="Download SO requested achievements">
                                                    <i class="bi bi-download"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Objectives">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="btnViewThisRequest" runat="server"
                                                CssClass="btn btn-outline-info ml-1"
                                                CommandArgument='<%# Eval("RequestId") + "," + Eval("Status") + "," + Eval("SOCode") + "," + Eval("PCYear") + "," + Eval("Quarter") %>'
                                                OnClick="btnViewThisRequest_Click"
                                                OnClientClick="showLoader()"
                                                ToolTip="View this request">
                                                    <i class="bi bi-eye"></i>
                                            </asp:LinkButton>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>

            </div>
        </div>

        <%-- Modal for View --%>
        <div class="modal fade" id="exampleModalCenter" tabindex="-1" role="dialog" aria-labelledby="exampleModalCenterTitle" aria-hidden="true">
            <div class="modal-dialog modal-dialog-centered modal-lg" role="document">
                <div class="modal-content">
                    <div class="modal-header">
                        <h5 class="modal-title" id="exampleModalLongTitle" runat="server"></h5>
                        <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <div class="modal-body py-2 px-0">
                        <div class="px-3">

                            <!-- Objectives Section -->
                            <div id="ObjectivesDiv" runat="server" visible="false">
                                <div class="">
                                    <div class="dashboard-card">

                                        <%--<h6 class="dashboard-card-title">Objectives</h6>--%>
                                        <label class="font-weight-bold d-block mb-2">Objectives</label>

                                        <div class="row">

                                            <!-- Significant Achievement -->
                                            <div class="col-12 col-md-6 mb-3">
                                                <label class="font-weight-semibold">Significant Achievement</label>
                                                <asp:TextBox ID="txtSigAchi" runat="server"
                                                    CssClass="form-control"
                                                    placeholder="Enter Significant Achievement" ReadOnly="true" />
                                            </div>

                                            <!-- JOB & Personal Development -->
                                            <div class="col-12 col-md-6 mb-3">
                                                <label class="font-weight-semibold">JOB & Personal Development</label>
                                                <asp:TextBox ID="txtPerDev" runat="server"
                                                    CssClass="form-control"
                                                    placeholder="Enter JOB & Personal Development" ReadOnly="true" />
                                            </div>

                                            <!-- Career Development and Ambitions -->
                                            <div class="col-12 col-md-6 mb-3">
                                                <label class="font-weight-semibold">Career Development and Ambitions</label>
                                                <asp:TextBox ID="txtCarDevAmb" runat="server"
                                                    CssClass="form-control"
                                                    placeholder="Enter Career Development and Ambitions" ReadOnly="true" />
                                            </div>

                                            <!-- KCA Section -->
                                            <div class="col-12 mb-3">
                                                <label class="font-weight-bold d-block mb-2">KCA</label>

                                                <div class="row">

                                                    <!-- Row 1 -->
                                                    <!-- WIPRO Values-->
                                                    <div class="col-md-6 mb-3">
                                                        <div class="kca-card p-3">

                                                            <!-- Title -->
                                                            <div class="d-flex justify-content-between align-items-center mb-2">
                                                                <label class="mb-0 font-weight-bold">Wipro Values</label>
                                                                <span class="badge badge-light">Out of 4</span>
                                                            </div>

                                                            <div class="row align-items-center">

                                                                <!-- OLD (SO Given) -->
                                                                <div class="col-5 text-center old-rating">
                                                                    <small class="text-muted d-block">SO Given</small>

                                                                    <div class="rating-stars old-stars" id="oldWiproStars"></div>

                                                                    <!-- ⭐ VALUE -->
                                                                    <asp:Label ID="lblOldWipro"
                                                                        runat="server"
                                                                        CssClass="rating-value old-value d-block"></asp:Label>
                                                                </div>

                                                                <!-- Arrow -->
                                                                <div class="col-2 text-center">
                                                                    <i class="bi bi-arrow-right text-primary" style="font-size: 18px;"></i>
                                                                </div>

                                                                <!-- NEW (Manager Rating) -->
                                                                <div class="col-5 text-center new-rating">
                                                                    <small class="text-success d-block">Your Rating</small>

                                                                    <div class="rating-stars rating-group form-control border-0 justify-content-center"
                                                                        data-target="hdnWiproValues">
                                                                        <i class="bi bi-star rating-star" data-value="1"></i>
                                                                        <i class="bi bi-star rating-star" data-value="2"></i>
                                                                        <i class="bi bi-star rating-star" data-value="3"></i>
                                                                        <i class="bi bi-star rating-star" data-value="4"></i>
                                                                    </div>

                                                                    <!-- ⭐ VALUE -->
                                                                    <small class="rating-text rating-value d-block mt-1"></small>
                                                                </div>

                                                            </div>

                                                            <!-- Difference Indicator -->
                                                            <div class="text-center mt-2">
                                                                <asp:Label ID="lblDiffWipro" runat="server" CssClass="small font-weight-bold"></asp:Label>
                                                            </div>

                                                        </div>

                                                        <asp:HiddenField ID="hdnWiproValues" runat="server" ClientIDMode="Static" />
                                                    </div>

                                                    <!-- 🔹 LEADING PEOPLE -->
                                                    <div class="col-md-6 mb-3">
                                                        <div class="kca-card p-3">
                                                            <div class="d-flex justify-content-between mb-2">
                                                                <label class="font-weight-bold">Leading People</label>
                                                                <span class="badge badge-light">Out of 4</span>
                                                            </div>

                                                            <div class="row align-items-center">
                                                                <div class="col-5 text-center old-rating">
                                                                    <small>SO Given</small>
                                                                    <div class="rating-stars old-stars" id="oldLeadingStars"></div>
                                                                    <asp:Label ID="lblOldLeading" runat="server" CssClass="rating-value old-value"></asp:Label>
                                                                </div>

                                                                <div class="col-2 text-center">
                                                                    <i class="bi bi-arrow-right text-primary"></i>
                                                                </div>

                                                                <div class="col-5 text-center new-rating">
                                                                    <small class="text-success d-block">Your Rating</small>
                                                                    <div class="rating-stars rating-group form-control border-0"
                                                                        data-target="hdnLeadingPeople">
                                                                        <i class="bi bi-star rating-star" data-value="1"></i>
                                                                        <i class="bi bi-star rating-star" data-value="2"></i>
                                                                        <i class="bi bi-star rating-star" data-value="3"></i>
                                                                        <i class="bi bi-star rating-star" data-value="4"></i>
                                                                    </div>
                                                                    <small class="rating-text rating-value d-block mt-1"></small>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <asp:HiddenField ID="hdnLeadingPeople" runat="server" ClientIDMode="Static" />
                                                    </div>

                                                    <!-- Row 2 -->
                                                    <!-- 🔹 EXECUTION -->
                                                    <div class="col-md-6 mb-3">
                                                        <div class="kca-card p-3">
                                                            <div class="d-flex justify-content-between mb-2">
                                                                <label class="font-weight-bold">Execution Excellence</label>
                                                                <span class="badge badge-light">Out of 4</span>
                                                            </div>

                                                            <div class="row align-items-center">
                                                                <div class="col-5 text-center">
                                                                    <small>SO Given</small>
                                                                    <div class="rating-stars old-stars" id="oldExecutionStars"></div>
                                                                    <asp:Label ID="lblOldExecution" runat="server" CssClass="rating-value old-value"></asp:Label>
                                                                </div>

                                                                <div class="col-2 text-center">
                                                                    <i class="bi bi-arrow-right text-primary"></i>
                                                                </div>

                                                                <div class="col-5 text-center new-rating">
                                                                    <small class="text-success d-block">Your Rating</small>
                                                                    <div class="rating-stars rating-group form-control border-0"
                                                                        data-target="hdnExecution">

                                                                        <i class="bi bi-star rating-star" data-value="1"></i>
                                                                        <i class="bi bi-star rating-star" data-value="2"></i>
                                                                        <i class="bi bi-star rating-star" data-value="3"></i>
                                                                        <i class="bi bi-star rating-star" data-value="4"></i>
                                                                    </div>
                                                                    <small class="rating-text rating-value d-block mt-1"></small>
                                                                </div>
                                                            </div>
                                                        </div>
                                                        <asp:HiddenField ID="hdnExecution" runat="server" ClientIDMode="Static" />
                                                    </div>

                                                    <%--PASSION FOR RESULT--%>
                                                    <div class="col-md-6 mb-3">
                                                        <div class="kca-card p-3">
                                                            <div class="d-flex justify-content-between mb-2">
                                                                <label class="font-weight-bold">Passion for Result</label>
                                                                <span class="badge badge-light">Out of 4</span>
                                                            </div>

                                                            <div class="row align-items-center">

                                                                <div class="col-5 text-center old-rating">
                                                                    <small>SO Given</small>
                                                                    <div class="rating-stars old-stars" id="oldPassionStars"></div>
                                                                    <asp:Label ID="lblOldPassion" runat="server" CssClass="rating-value old-value"></asp:Label>
                                                                </div>

                                                                <div class="col-2 text-center">
                                                                    <i class="bi bi-arrow-right text-primary"></i>
                                                                </div>

                                                                <div class="col-5 text-center new-rating">
                                                                    <small class="text-success d-block">Your Rating</small>
                                                                    <div class="rating-stars rating-group form-control border-0"
                                                                        data-target="hdnPassion">
                                                                        <i class="bi bi-star rating-star" data-value="1"></i>
                                                                        <i class="bi bi-star rating-star" data-value="2"></i>
                                                                        <i class="bi bi-star rating-star" data-value="3"></i>
                                                                        <i class="bi bi-star rating-star" data-value="4"></i>
                                                                    </div>
                                                                    <small class="rating-text rating-value d-block mt-1"></small>
                                                                </div>

                                                            </div>
                                                        </div>

                                                        <asp:HiddenField ID="hdnPassion" runat="server" ClientIDMode="Static" />
                                                    </div>

                                                    <!-- Row 3 -->
                                                    <%--COLLABORATIVE WORKING--%>
                                                    <div class="col-md-6 mb-3">
                                                        <div class="kca-card p-3">
                                                            <div class="d-flex justify-content-between mb-2">
                                                                <label class="font-weight-bold">Collaborative Working</label>
                                                                <span class="badge badge-light">Out of 4</span>
                                                            </div>

                                                            <div class="row align-items-center">

                                                                <div class="col-5 text-center">
                                                                    <small>SO Given</small>
                                                                    <div class="rating-stars old-stars" id="oldCollabStars"></div>
                                                                    <asp:Label ID="lblOldCollab" runat="server" CssClass="rating-value old-value"></asp:Label>
                                                                </div>

                                                                <div class="col-2 text-center">
                                                                    <i class="bi bi-arrow-right text-primary"></i>
                                                                </div>

                                                                <div class="col-5 text-center new-rating">
                                                                    <small class="text-success d-block">Your Rating</small>
                                                                    <div class="rating-stars rating-group form-control border-0"
                                                                        data-target="hdnCollab">
                                                                        <i class="bi bi-star rating-star" data-value="1"></i>
                                                                        <i class="bi bi-star rating-star" data-value="2"></i>
                                                                        <i class="bi bi-star rating-star" data-value="3"></i>
                                                                        <i class="bi bi-star rating-star" data-value="4"></i>
                                                                    </div>
                                                                    <small class="rating-text rating-value d-block mt-1"></small>
                                                                </div>

                                                            </div>
                                                        </div>

                                                        <asp:HiddenField ID="hdnCollab" runat="server" ClientIDMode="Static" />
                                                    </div>

                                                    <%--CUSTOMER ORIENTATION--%>
                                                    <div class="col-md-6 mb-3">
                                                        <div class="kca-card p-3">
                                                            <div class="d-flex justify-content-between mb-2">
                                                                <label class="font-weight-bold">Customer Orientation</label>
                                                                <span class="badge badge-light">Out of 4</span>
                                                            </div>

                                                            <div class="row align-items-center">

                                                                <div class="col-5 text-center">
                                                                    <small>SO Given</small>
                                                                    <div class="rating-stars old-stars" id="oldCustomerStars"></div>
                                                                    <asp:Label ID="lblOldCustomer" runat="server" CssClass="rating-value old-value"></asp:Label>
                                                                </div>

                                                                <div class="col-2 text-center">
                                                                    <i class="bi bi-arrow-right text-primary"></i>
                                                                </div>

                                                                <div class="col-5 text-center new-rating">
                                                                    <small class="text-success d-block">Your Rating</small>
                                                                    <div class="rating-stars rating-group form-control border-0"
                                                                        data-target="hdnCustomer">
                                                                        <i class="bi bi-star rating-star" data-value="1"></i>
                                                                        <i class="bi bi-star rating-star" data-value="2"></i>
                                                                        <i class="bi bi-star rating-star" data-value="3"></i>
                                                                        <i class="bi bi-star rating-star" data-value="4"></i>
                                                                    </div>
                                                                    <small class="rating-text rating-value d-block mt-1"></small>
                                                                </div>

                                                            </div>
                                                        </div>

                                                        <asp:HiddenField ID="hdnCustomer" runat="server" ClientIDMode="Static" />
                                                    </div>

                                                </div>
                                            </div>

                                        </div>

                                    </div>
                                </div>
                            </div>

                            <!-- Remarks -->
                            <div class="form-group mb-0" id="RemarksDiv" runat="server" visible="false">
                                <asp:TextBox ID="txtRemarks" runat="server"
                                    CssClass="form-control text-left"
                                    TextMode="MultiLine"
                                    Rows="1"
                                    placeholder="Write a valid reason/feedback here..."
                                    ReadOnly="true" />
                            </div>

                        </div>
                    </div>

                    <div class="modal-footer">
                        <asp:Button ID="ForwardBtn" runat="server" Text="Forward" CssClass="btn btn-success" OnClick="Forward_Click" OnClientClick="showLoader()" />
                        <asp:Button ID="UpdateBtn" runat="server" Text="Update" CssClass="btn btn-success" OnClick="UpdateBtn_Click" OnClientClick="showLoader()" />
                        <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    </div>
                </div>
            </div>
        </div>


        <div class="container">
            <div class="row mt-3">
                <div class="alert alert-success alert-box" role="alert" id="ApproveAlert" style="display: none;">
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <h4 class="alert-heading">Approve Alert!</h4>
                        <button type="button" class="close" aria-label="Close" onclick="hideAlert(); return false;">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <hr />
                    <p class="mb-0">Do you want to Approve?</p>
                    <div class="alert-footer mt-3 d-flex justify-content-end">
                        <asp:Button ID="ApproveAlertButton" runat="server" Text="Approve" CssClass="btn btn-success" OnClientClick="showLoader()" OnClick="btnRowApprove_Click" />
                        <button type="button" class="btn btn-secondary ml-2" onclick="hideAlert(); return false;">Cancel</button>
                    </div>
                </div>

                <div class="alert alert-danger alert-box" role="alert" id="RejectAlert" style="display: none;">
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <h4 class="alert-heading">Reject Alert!</h4>
                        <button type="button" class="close" aria-label="Close" onclick="hideAlert(); return false;">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <hr />
                    <p class="mb-0">Do you want to Reject?</p>
                    <div class="alert-footer mt-3 d-flex justify-content-end">
                        <asp:Button ID="RejectButton" runat="server" Text="Reject" CssClass="btn btn-danger" OnClientClick="showLoader()" OnClick="btnRowReject_Click" />
                        <button type="button" class="btn btn-secondary ml-2" onclick="hideAlert(); return false;">Cancel</button>
                    </div>
                </div>

                <div class="alert alert-success alert-box" role="alert" id="ApproveSelectedAlert" style="display: none;">
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <h4 class="alert-heading">Approve Alert!</h4>
                        <button type="button" class="close" aria-label="Close" onclick="hideAlert(); return false;">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <hr />
                    <p class="mb-0">Do you want to Approve?</p>
                    <div class="alert-footer mt-3 d-flex justify-content-end">
                        <asp:Button ID="Button1" runat="server" Text="Approve" CssClass="btn btn-success" OnClientClick="showLoader()" OnClick="ApproveSelectedBtn_Click" />
                        <button type="button" class="btn btn-secondary ml-2" onclick="hideAlert(); return false;">Cancel</button>
                    </div>
                </div>

                <div class="alert alert-danger alert-box" role="alert" id="ApproveRejectedAlert" style="display: none;">
                    <div style="display: flex; justify-content: space-between; align-items: center;">
                        <h4 class="alert-heading">Reject Alert!</h4>
                        <button type="button" class="close" aria-label="Close" onclick="hideAlert(); return false;">
                            <span aria-hidden="true">&times;</span>
                        </button>
                    </div>
                    <hr />
                    <p class="mb-0">Do you want to Reject?</p>
                    <div class="alert-footer mt-3 d-flex justify-content-end">
                        <asp:Button ID="Button2" runat="server" Text="Reject" CssClass="btn btn-danger" OnClientClick="showLoader()" OnClick="RejectSelectedBtn_Click" />
                        <button type="button" class="btn btn-secondary ml-2" onclick="hideAlert(); return false;">Cancel</button>
                    </div>
                </div>
            </div>
        </div>


        <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
        <asp:HiddenField ID="hdnBusinessType" runat="server" />
        <asp:HiddenField ID="hdnRole" runat="server" />
        <asp:HiddenField ID="hfSelectedRowData" runat="server" />
        <asp:HiddenField ID="hfApproveRequestId" runat="server" />
        <asp:HiddenField ID="hfRejectRequestId" runat="server" />
        <asp:HiddenField ID="hdnRequestId" runat="server" />

    </div>

    <%--script for select all in pending approval Grid--%>
    <script>
        $(document).ready(function () {
            const updateParentCheckbox = () => {
                const childCheckboxes = $(".child-checkbox");
                const checkedCheckboxes = childCheckboxes.filter(":checked");
                const parentCheckbox = $("#parentCheckbox");

                if (checkedCheckboxes.length === 0) {
                    parentCheckbox.prop("checked", false).prop("indeterminate", false);
                } else if (checkedCheckboxes.length === childCheckboxes.length) {
                    parentCheckbox.prop("checked", true).prop("indeterminate", false);
                } else {
                    parentCheckbox.prop("checked", false).prop("indeterminate", true);
                }
            };

            $("#parentCheckbox").on("change", function () {
                const isChecked = $(this).is(":checked");
                $(".child-checkbox").prop("checked", isChecked);
            });

            $(".child-checkbox").on("change", updateParentCheckbox);

            updateParentCheckbox(); // Initialize state on page load
        });
    </script>

    <%--script to handle alert--%>
    <script>
        function openTransferModal() {
            $('#exampleModalCenter').modal('show');
        }

        function showApproveAlert(requestId) {
            // Store RequestId in HiddenField
            document.getElementById('<%= hfApproveRequestId.ClientID %>').value = requestId;

            // Show alert
            document.getElementById('ApproveAlert').style.display = 'block';
        }

        function showRejectAlert(requestId) {
            // Store RequestId in HiddenField
            document.getElementById('<%= hfRejectRequestId.ClientID %>').value = requestId;

            // Show alert
            document.getElementById('RejectAlert').style.display = 'block';
        }

        function approveSelectedAlert() {
            document.getElementById("ApproveSelectedAlert").style.display = "block";
        }

        function approveRejectedAlert() {
            document.getElementById("ApproveRejectedAlert").style.display = "block";
        }

        function hideAlert() {
            document.getElementById("ApproveAlert").style.display = "none";
            document.getElementById("RejectAlert").style.display = "none";
            document.getElementById("ApproveSelectedAlert").style.display = "none";
            document.getElementById("ApproveRejectedAlert").style.display = "none";
        }
    </script>

    <script>
        function renderOldStars(containerId, rating) {
            var container = $('#' + containerId);
            container.empty();

            rating = parseFloat(rating) || 0;

            for (let i = 1; i <= 4; i++) {
                if (rating >= i) {
                    container.append('<i class="bi bi-star-fill"></i>');
                }
                else if (rating >= i - 0.5) {
                    container.append('<i class="bi bi-star-half"></i>');
                }
                else {
                    container.append('<i class="bi bi-star"></i>');
                }
            }
        }
    </script>

    <%--script to bind stars--%>
    <script>
        $(document).ready(function () {

            // ⭐ Mouse move (hover preview)
            $('.rating-group .rating-star').on('mousemove', function (e) {
                var star = $(this);
                var container = star.closest('.rating-group');

                var offset = star.offset();
                var width = star.width();
                var x = e.pageX - offset.left;

                var value = star.data('value');

                if (x < width / 2)
                    highlightStars(container, value - 0.5);
                else
                    highlightStars(container, value);
            });

            // ⭐ Click (final selection)
            $('.rating-group .rating-star').on('click', function (e) {
                var star = $(this);
                var container = star.closest('.rating-group');

                var offset = star.offset();
                var width = star.width();
                var x = e.pageX - offset.left;

                var value = star.data('value');

                if (x < width / 2)
                    value = value - 0.5;

                var target = container.data('target');

                // ✅ store in hidden field
                $('#' + target).val(value);

                // ✅ store in container
                container.data('selected', value);

                // ✅ update text
                container.closest('.new-rating').find('.rating-text')
                    .text(value + " / 4");

                // ✅ highlight stars
                highlightStars(container, value);
            });

            // ⭐ Restore selected rating on mouse leave
            $('.rating-group').on('mouseleave', function () {
                var selected = $(this).data('selected') || 0;
                highlightStars($(this), selected);
            });

        });


        // ⭐ Highlight stars (PER GROUP)
        function highlightStars(container, rating) {
            container.find('.rating-star').each(function () {
                var starValue = $(this).data('value');

                if (rating >= starValue) {
                    $(this).removeClass('bi-star bi-star-half')
                        .addClass('bi-star-fill text-warning');
                }
                else if (rating >= starValue - 0.5) {
                    $(this).removeClass('bi-star bi-star-fill')
                        .addClass('bi-star-half text-warning');
                }
                else {
                    $(this).removeClass('bi-star-fill bi-star-half text-warning')
                        .addClass('bi-star');
                }
            });
        }


        // ⭐ Set rating from C# (AUTO BIND)
        function setRatingGroup(hiddenId, rating) {

            var container = $('.rating-group[data-target="' + hiddenId + '"]');

            if (!container.length) return;

            rating = parseFloat(rating) || 0;

            // set hidden field
            $('#' + hiddenId).val(rating);

            // store selected value
            container.data('selected', rating);

            // update label
            container.closest('.new-rating').find('.rating-text')
                .text(rating + " / 4");

            // highlight
            highlightStars(container, rating);
        }

    </script>

    <script>
        const messages = [
            "Please wait...",
            "Processing your request...",
            "Almost there...",
            "Just a moment...",
            "Fetching data...",
            "Hang tight...",
            "Thanks for your patience...",
            "We appreciate your wait..."
        ];

        let index = 0;
        let interval;

        function showLoader() {
            document.getElementById("loaderContainer").style.display = "flex"; // Show loader

            // Start changing messages
            interval = setInterval(() => {
                document.getElementById("loadingText").innerText = messages[index];
                index = (index + 1) % messages.length;
            }, 3000);

        }

        function hideLoader() {
            clearInterval(interval);
            document.getElementById("loaderContainer").style.display = "none"; // Hide loader
        }
    </script>


</asp:Content>


