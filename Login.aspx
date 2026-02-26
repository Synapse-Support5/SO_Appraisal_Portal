<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Login.aspx.cs" Inherits="SO_Appraisal.Login" Async="true" %>

<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <title>Email OTP Login</title>
    <meta name="viewport" content="width=device-width, initial-scale=1">

    <!-- Bootstrap 5 -->
    <link href="https://cdn.jsdelivr.net/npm/bootstrap@5.3.2/dist/css/bootstrap.min.css" rel="stylesheet">

    <style>
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

        body {
            min-height: 100vh;
            display: flex;
            align-items: center;
            justify-content: center;
        }

        /* OUTER DASHBOARD WRAPPER */
        .dashboard-wrapper {
            background: #dce3ea;
            border-radius: 10px;
            padding: 25px;
            box-shadow: 0 4px 10px rgba(0,0,0,0.08);
            width: 100%;
            max-width: 450px;
        }

        /* INNER DASHBOARD CARD */
        .dashboard-card {
            background: #ffffff;
            border-radius: 8px;
            padding: 25px;
            box-shadow: 0 2px 6px rgba(0,0,0,0.06);
            height: 100%;
        }

        .login-title {
            font-weight: 600;
            color: #4e73df;
        }

        .form-control {
            border-radius: 6px;
        }

        .btn-primary {
            background-color: #4e73df;
            border: none;
            border-radius: 6px;
            transition: 0.3s ease;
        }

            .btn-primary:hover {
                background-color: #2e59d9;
            }

        /*.otp-section {
            display: none;
        }*/

        .resend-link {
            font-size: 14px;
            color: #858796;
            cursor: pointer;
        }

            .resend-link:hover {
                text-decoration: underline;
            }

        @media (max-width: 576px) {
            .dashboard-wrapper {
                padding: 15px;
            }

            .dashboard-card {
                padding: 20px;
            }
        }
    </style>

    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://maxcdn.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.min.js"></script>
    <script src="https://stackpath.bootstrapcdn.com/bootstrap/4.5.2/js/bootstrap.bundle.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/moment.js/2.29.1/moment.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/bootstrap-datepicker/1.9.0/js/bootstrap-datepicker.min.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/html2canvas/1.4.1/html2canvas.min.js"></script>
    <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.min.js"></script>

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
</head>
<body>

    <form id="form1" runat="server">

        <div class="container">
            <div class="dashboard-wrapper mx-auto">
                <div class="dashboard-card">

                    <h4 class="text-center login-title mb-4">Secure Login</h4>

                    <!-- Email Section -->
                    <div id="emailSection" runat="server" visible="true">
                        <div class="mb-3">
                            <label class="form-label">Email Address</label>
                            <asp:TextBox ID="EmailTxt" type="email" runat="server" CssClass="form-control" Placeholder="Enter your email" required="true"></asp:TextBox>
                        </div>

                        <asp:Button Text="Send OTP" ID="SendOTPBtn" runat="server" class="btn btn-primary w-100" OnClick="SendOTPBtn_Click" />
                    </div>

                    <!-- OTP Section -->
                    <div id="otpSection" class="otp-section" runat="server" visible="false">
                        <div class="mb-3">
                            <label class="form-label">Enter OTP</label>
                            <asp:TextBox ID="EnterOtpTxt" runat="server" class="form-control" placeholder="Enter 6-digit OTP"></asp:TextBox>
                        </div>

                        <asp:Button ID="VerifyLoginBtn" runat="server" class="btn btn-primary w-100 mb-3" Text="Verify & Login" OnClick="VerifyLoginBtn_Click"></asp:Button>

                        <div class="text-center">
                            <span class="resend-link">Resend OTP</span>
                        </div>
                    </div>

                </div>
            </div>
        </div>



        <div id="toastContainer" aria-live="polite" aria-atomic="true" style="position: relative; min-height: 200px;"></div>
        <asp:HiddenField ID="hdnBusinessType" runat="server" />
        <asp:HiddenField ID="hdnRole" runat="server" />

    </form>


</body>
</html>
