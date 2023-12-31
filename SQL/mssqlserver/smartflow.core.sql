CREATE DATABASE [Smartflow.Core]
GO
USE [Smartflow.Core]
GO
/****** Object:  UserDefinedFunction [dbo].[F_GetActorList]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[F_GetActorList]
(	
	-- Add the parameters for the function here
	 @TaskId bigint
)
RETURNS varchar(128)  
AS
BEGIN

declare @AuthCode varchar(50),@code varchar(2048)
declare my_cur cursor for select [AuthCode] from  t_wf_task_auth WHERE TaskId=@TaskId
set @code=''
open my_cur
fetch next from my_cur into @AuthCode
while @@fetch_status=0
begin
  
	  select  @code=@code+','+[dbo].[F_GetUserName](userId) from (

	  select  UID AS userId from [dbo].[t_sys_umr] where RID=@AuthCode
	  union 
	  select ID  AS userId  from [dbo].[t_sys_user] where id=@AuthCode
	  union 
	  select ID  AS userId  from [dbo].[t_sys_user] where OrganizationCode=@AuthCode) as t



fetch next from my_cur into @authCode

end
close my_cur
deallocate my_cur



if(len(@code)>0)
begin
set @code=right(@code,len(@code)-1)
end
return @code

END

GO
/****** Object:  UserDefinedFunction [dbo].[F_GetOrganizationName]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE FUNCTION [dbo].[F_GetOrganizationName]
(	
	-- Add the parameters for the function here
	 @OrganizationCode varchar(50)
)
RETURNS varchar(128)  
AS
BEGIN
    DECLARE @result varchar(128)
	SELECT @result=[Name] FROM [dbo].[t_sys_organization] WHERE [ID]=@OrganizationCode
    RETURN @result;
END
GO
/****** Object:  UserDefinedFunction [dbo].[F_GetRoleGroup]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create FUNCTION [dbo].[F_GetRoleGroup]
(	
	-- Add the parameters for the function here
	 @UID varchar(50)
)
RETURNS varchar(512)  
AS
BEGIN

declare @Names varchar(1024)
set @Names=''
select @Names=@Names+[dbo].[F_GetRoleName](RID)+','  from [dbo].[t_sys_umr] where  [UID]=@UID
if(len(@Names)>0)
begin
set @Names=left(@Names,len(@Names)-1)
end

return @Names
END
GO
/****** Object:  UserDefinedFunction [dbo].[F_GetRoleName]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create FUNCTION [dbo].[F_GetRoleName]
(	
	-- Add the parameters for the function here
	 @RID varchar(50)
)
RETURNS varchar(128)  
AS
BEGIN


    DECLARE @result varchar(128)
	SELECT @result=[NAME] FROM [dbo].[t_sys_role] WHERE [ID]=@RID
   
    RETURN @result

END
GO
/****** Object:  UserDefinedFunction [dbo].[F_GetUserName]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
create FUNCTION [dbo].[F_GetUserName]
(	
	-- Add the parameters for the function here
	 @UID varchar(50)
)
RETURNS varchar(128)  
AS
BEGIN


    DECLARE @result varchar(128)
	SELECT @result=[NAME] FROM [dbo].[t_sys_user] WHERE ID=@UID
   
    RETURN @result

END
GO
/****** Object:  Table [dbo].[t_wf_template]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_wf_template](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](256) NULL,
	[Source] [text] NULL,
	[Status] [int] NULL,
	[Version] [decimal](18, 1) NULL,
	[CreateTime] [datetime] NULL,
	[UpdateTime] [datetime] NULL,
	[CategoryName] [varchar](50) NULL,
	[CategoryId] [int] NULL,
	[Memo] [text] NULL,
 CONSTRAINT [PK_t_wf_bpmn] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_wf_instance]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_wf_instance](
	[Id] [varchar](50) NULL,
	[TemplateId] [bigint] NULL,
	[CategoryId] [int] NULL,
	[CreateTime] [datetime] NULL,
	[Creator] [varchar](50) NULL,
	[BusinessId] [varchar](50) NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_wf_task]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_wf_task](
	[Id] [bigint] NOT NULL,
	[Name] [varchar](50) NOT NULL,
	[Code] [varchar](50) NOT NULL,
	[ParentId] [bigint] NULL,
	[InstanceId] [varchar](36) NULL,
	[Status] [int] NULL,
	[Type] [varchar](50) NULL,
	[CreateTime] [datetime] NULL,
	[LineCode] [varchar](50) NULL,
	[Creator] [varchar](50) NULL,
	[Parallel] [int] NULL,
 CONSTRAINT [PK_t_wf_task] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  View [dbo].[vw_task]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO







CREATE view [dbo].[vw_task]
as 

	SELECT  t.[Id]
		,t.[Name]
		,t.[Code]
		,[ParentId]
		,t.[InstanceId]
		,t.[Status]
		,[Type]
		,t.[CreateTime]
		,[LineCode]
		,[Parallel]
		,m.CategoryName
		,m.CategoryId
		,[dbo].[F_GetUserName](i.Creator) AS Sender
		,Concat(m.Name,'-',m.Version) TemplateName,
		[dbo].[F_GetActorList](t.Id) Actor
		FROM [Smartflow.Core].[dbo].[t_wf_task] t,t_wf_template m,t_wf_instance i
		where t.Status=0 and t.Type not in ('Fork','Join') and t.InstanceId=i.Id and m.Id=i.TemplateId   
GO
/****** Object:  Table [dbo].[t_sys_organization]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_sys_organization](
	[ID] [varchar](50) NOT NULL,
	[ParentID] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[Memo] [text] NULL,
 CONSTRAINT [PK_t_sys_organization] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_sys_rmm]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_sys_rmm](
	[ID] [varchar](50) NOT NULL,
	[RID] [varchar](50) NULL,
	[MID] [varchar](50) NULL,
 CONSTRAINT [PK_t_sys_rmm] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_sys_role]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_sys_role](
	[ID] [varchar](50) NOT NULL,
	[Name] [varchar](50) NULL,
	[Memo] [text] NULL,
	[Type] [int] NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_t_sys_role] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_sys_umr]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_sys_umr](
	[ID] [varchar](50) NOT NULL,
	[RID] [varchar](50) NULL,
	[UID] [varchar](50) NULL,
 CONSTRAINT [PK_t_sys_umr] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_sys_user]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_sys_user](
	[ID] [varchar](50) NOT NULL,
	[UID] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[No] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[OrganizationCode] [varchar](50) NULL,
	[Type] [int] NULL,
	[Mail] [varchar](50) NULL,
	[Status] [int] NULL,
	[Error] [int] NULL,
	[CreateTime] [datetime] NULL,
	[LatestTime] [datetime] NULL,
	[Force] [int] NULL,
	[Memo] [text] NULL,
 CONSTRAINT [PK_t_sys_user] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_vacation]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_vacation](
	[NID] [varchar](50) NOT NULL,
	[Name] [varchar](50) NULL,
	[Day] [int] NULL,
	[Reason] [text] NULL,
	[CreateTime] [datetime] NULL,
	[VacationType] [varchar](50) NULL,
	[UID] [varchar](50) NULL,
	[InstanceID] [varchar](50) NULL,
	[TemplateID] [bigint] NULL,
 CONSTRAINT [PK_t_vacation] PRIMARY KEY CLUSTERED 
(
	[NID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_wf_category]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_wf_category](
	[ID] [int] NOT NULL,
	[Name] [varchar](50) NULL,
	[Url] [varchar](512) NULL,
	[Sort] [int] NULL,
 CONSTRAINT [PK_t_wf_category] PRIMARY KEY CLUSTERED 
(
	[ID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_wf_mail]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_wf_mail](
	[Id] [int] NOT NULL,
	[Account] [varchar](50) NULL,
	[Password] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[Host] [varchar](50) NULL,
	[Port] [int] NULL,
	[EnableSsl] [int] NULL,
 CONSTRAINT [PK_t_mail] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_wf_record]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_wf_record](
	[Id] [bigint] NOT NULL,
	[Code] [varchar](50) NULL,
	[Name] [varchar](50) NULL,
	[Comment] [text] NULL,
	[InstanceId] [varchar](50) NULL,
	[LineName] [varchar](50) NULL,
	[Creator] [varchar](50) NULL,
	[CreateTime] [datetime] NULL,
 CONSTRAINT [PK_t_record] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[t_wf_task_auth]    Script Date: 2023-08-20 8:29:52 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[t_wf_task_auth](
	[Id] [bigint] NOT NULL,
	[TaskId] [bigint] NULL,
	[AuthCode] [varchar](50) NULL,
	[Type] [int] NULL,
 CONSTRAINT [PK_t_wf_task_auth] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[t_sys_rmm] ADD  CONSTRAINT [DF_t_sys_rmm_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[t_sys_rmm] ADD  CONSTRAINT [DF_t_sys_rmm_MID]  DEFAULT (newid()) FOR [MID]
GO
ALTER TABLE [dbo].[t_sys_role] ADD  CONSTRAINT [DF_t_sys_role_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[t_sys_role] ADD  CONSTRAINT [DF_t_sys_role_Type]  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [dbo].[t_sys_role] ADD  CONSTRAINT [DF_t_sys_role_CreateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[t_sys_umr] ADD  CONSTRAINT [DF_t_sys_umr_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[t_sys_user] ADD  CONSTRAINT [DF_t_sys_user_ID]  DEFAULT (newid()) FOR [ID]
GO
ALTER TABLE [dbo].[t_sys_user] ADD  CONSTRAINT [DF_t_sys_user_Type]  DEFAULT ((0)) FOR [Type]
GO
ALTER TABLE [dbo].[t_sys_user] ADD  CONSTRAINT [DF_t_sys_user_Status]  DEFAULT ((0)) FOR [Status]
GO
ALTER TABLE [dbo].[t_sys_user] ADD  CONSTRAINT [DF_t_sys_user_Error]  DEFAULT ((0)) FOR [Error]
GO
ALTER TABLE [dbo].[t_sys_user] ADD  CONSTRAINT [DF_t_sys_user_CreateDateTime]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[t_sys_user] ADD  CONSTRAINT [DF_t_sys_user_LatestTime]  DEFAULT (getdate()) FOR [LatestTime]
GO
ALTER TABLE [dbo].[t_sys_user] ADD  CONSTRAINT [DF_t_sys_user_Force]  DEFAULT ((0)) FOR [Force]
GO
ALTER TABLE [dbo].[t_wf_mail] ADD  CONSTRAINT [DF_t_mail_EnableSsl]  DEFAULT ((0)) FOR [EnableSsl]
GO
ALTER TABLE [dbo].[t_wf_record] ADD  CONSTRAINT [DF_t_record_InsertDate]  DEFAULT (getdate()) FOR [CreateTime]
GO
ALTER TABLE [dbo].[t_wf_task] ADD  CONSTRAINT [DF_t_wf_task_Parallel]  DEFAULT ((0)) FOR [Parallel]
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:普通 1：系统生成' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_sys_role', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户类型（1:管理员 0：普通用）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_sys_user', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'用户状态（0：正常 1：禁用  2：删除）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_sys_user', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'错误记数' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_sys_user', @level2type=N'COLUMN',@level2name=N'Error'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'创建日期' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_sys_user', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'最新登录时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_sys_user', @level2type=N'COLUMN',@level2name=N'LatestTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'强制修改密码 0:不强制修改密码 1：强制修改密码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_sys_user', @level2type=N'COLUMN',@level2name=N'Force'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'申请时间' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_vacation', @level2type=N'COLUMN',@level2name=N'CreateTime'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'请假类型（事假、病假、产假）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_vacation', @level2type=N'COLUMN',@level2name=N'VacationType'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'类型名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_category', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'账户名' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_mail', @level2type=N'COLUMN',@level2name=N'Account'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'密码（授权码）' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_mail', @level2type=N'COLUMN',@level2name=N'Password'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'发送邮件显示的名称' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_mail', @level2type=N'COLUMN',@level2name=N'Name'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'服务器smtp.163.com' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_mail', @level2type=N'COLUMN',@level2name=N'Host'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'端口(25)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_mail', @level2type=N'COLUMN',@level2name=N'Port'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N' 启用HTTPS 0：http 1:https' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_mail', @level2type=N'COLUMN',@level2name=N'EnableSsl'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'节点编码' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_task', @level2type=N'COLUMN',@level2name=N'Code'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0：打开 1：关闭' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_task', @level2type=N'COLUMN',@level2name=N'Status'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'任务类型 Node 普通任务' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_task', @level2type=N'COLUMN',@level2name=N'Type'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:串行 1：并行' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_task', @level2type=N'COLUMN',@level2name=N'Parallel'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0:角色 1：人员 2: 单位' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_task_auth', @level2type=N'COLUMN',@level2name=N'Id'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'0：未启用 1：启用' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N't_wf_template', @level2type=N'COLUMN',@level2name=N'Status'
GO


