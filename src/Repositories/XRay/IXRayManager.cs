namespace Repositories.XRay
{
    public interface IXRayManager
    {
        void BeginXRaySegment(string segmentName);

        void EndXRaySegment();

    }
}