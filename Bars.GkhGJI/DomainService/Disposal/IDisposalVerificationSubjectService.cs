namespace Bars.GkhGji.DomainService
{
    using Bars.B4;

    public interface IDisposalVerificationSubjectService
    {
        IDataResult AddSurveySubjects(BaseParams baseParams);
        IDataResult AddSurveySubjects(long documentId, long[] ids);
    }
}
