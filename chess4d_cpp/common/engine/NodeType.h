#pragma once

namespace tgreiner::amy::common::engine
{
    class NodeType final
    {
    public:
        constexpr const NodeType& getSiblingType() const noexcept
        {
            switch (kind_)
            {
            case Kind::PV:
                return PV;
            case Kind::CUT:
                return ALL;
            case Kind::ALL:
                return CUT;
            }

            return PV;
        }

        friend constexpr bool operator==(const NodeType&, const NodeType&) = default;

        static const NodeType PV;
        static const NodeType CUT;
        static const NodeType ALL;

    private:
        enum class Kind
        {
            PV,
            CUT,
            ALL
        };

        constexpr explicit NodeType(Kind kind) noexcept
            : kind_(kind)
        {
        }

        Kind kind_;
    };

    inline constexpr NodeType NodeType::PV(NodeType::Kind::PV);
    inline constexpr NodeType NodeType::CUT(NodeType::Kind::CUT);
    inline constexpr NodeType NodeType::ALL(NodeType::Kind::ALL);
}
